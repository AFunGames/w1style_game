using UnityEngine;
using W1Style.Features.Combat.Components;
using W1Style.Features.Combat.Domain;
using W1Style.Features.Combat.Noise;
using W1Style.Features.Combat.Player;

namespace W1Style.Features.Combat.Enemy
{
    /// <summary>
    /// Simple enemy state machine with physics-based reactions.
    /// States: Idle → Alert → Dead / Unconscious.
    /// Listens for noise events and reacts to damage.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(NoiseListener))]
    public sealed class EnemyController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 2.5f;
        [SerializeField] private float _alertMoveSpeed = 4f;

        [Header("Detection")]
        [SerializeField] private float _sightRange = 12f;
        [SerializeField] private float _sightAngle = 60f;
        [SerializeField] private LayerMask _sightLayers = ~0;

        [Header("Physics Reaction")]
        [SerializeField] private float _knockdownForceThreshold = 10f;
        [SerializeField] private float _unconsciousTime = 4f;

        [Header("Armored")]
        [SerializeField] private bool _isArmored;
        [SerializeField] private float _armoredKnockdownMultiplier = 2f;

        private Rigidbody _rb;
        private Damageable _damageable;
        private NoiseListener _noiseListener;
        private EnemyState _state = EnemyState.Idle;
        private float _unconsciousTimer;
        private Transform _alertTarget;

        public EnemyState State => _state;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _damageable = GetComponent<Damageable>();
            _noiseListener = GetComponent<NoiseListener>();

            _rb.freezeRotation = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;

            _damageable.OnDamaged += HandleDamage;
            _damageable.OnDied += HandleDeath;
            _noiseListener.OnNoiseHeard += HandleNoise;
        }

        private void OnDestroy()
        {
            if (_damageable != null)
            {
                _damageable.OnDamaged -= HandleDamage;
                _damageable.OnDied -= HandleDeath;
            }
            if (_noiseListener != null)
                _noiseListener.OnNoiseHeard -= HandleNoise;
        }

        private void Update()
        {
            switch (_state)
            {
                case EnemyState.Idle:
                    UpdateIdle();
                    break;
                case EnemyState.Alert:
                    UpdateAlert();
                    break;
                case EnemyState.Unconscious:
                    UpdateUnconscious();
                    break;
                case EnemyState.Dead:
                    break;
            }
        }

        private void UpdateIdle()
        {
            // Simple sight check for player
            var player = FindPlayerInSight();
            if (player != null)
                BecomeAlert(player);
        }

        private void UpdateAlert()
        {
            if (_alertTarget == null)
            {
                _state = EnemyState.Idle;
                return;
            }

            // Face and move toward target
            Vector3 direction = (_alertTarget.position - transform.position);
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction.normalized);
                if (direction.magnitude > 2f)
                {
                    Vector3 velocity = _rb.linearVelocity;
                    Vector3 move = direction.normalized * _alertMoveSpeed;
                    velocity.x = move.x;
                    velocity.z = move.z;
                    _rb.linearVelocity = velocity;
                }
            }
        }

        private void UpdateUnconscious()
        {
            _unconsciousTimer -= Time.deltaTime;
            if (_unconsciousTimer <= 0f)
            {
                _state = EnemyState.Idle;
                _rb.freezeRotation = true;
            }
        }

        private void HandleDamage(DamageInfo info)
        {
            if (_state == EnemyState.Dead) return;

            // Check for knockdown
            float threshold = _isArmored
                ? _knockdownForceThreshold * _armoredKnockdownMultiplier
                : _knockdownForceThreshold;

            if (info.ImpulseForce >= threshold)
            {
                _state = EnemyState.Unconscious;
                _unconsciousTimer = _unconsciousTime;
                _rb.freezeRotation = false;
                NoiseEmitter.EmitNoise(transform.position, 8f);
            }
            else if (_state == EnemyState.Idle)
            {
                BecomeAlert(info.Source != null ? info.Source.transform : null);
            }
        }

        private void HandleDeath()
        {
            _state = EnemyState.Dead;
            _rb.freezeRotation = false;
            NoiseEmitter.EmitNoise(transform.position, 10f);
        }

        private void HandleNoise(Vector3 noisePosition)
        {
            if (_state != EnemyState.Idle) return;

            // Create a temporary target at the noise position
            var go = new GameObject("NoiseTarget");
            go.transform.position = noisePosition;
            BecomeAlert(go.transform);
            Destroy(go, 5f);
        }

        private void BecomeAlert(Transform target)
        {
            _state = EnemyState.Alert;
            _alertTarget = target;
        }

        private Transform FindPlayerInSight()
        {
            var colliders = UnityEngine.Physics.OverlapSphere(
                transform.position, _sightRange, _sightLayers);

            foreach (var col in colliders)
            {
                if (col.GetComponent<PlayerController>() == null) continue;

                Vector3 dirToPlayer = col.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, dirToPlayer);
                if (angle > _sightAngle) continue;

                if (UnityEngine.Physics.Raycast(
                    transform.position + Vector3.up,
                    dirToPlayer.normalized,
                    out RaycastHit hit, _sightRange, _sightLayers))
                {
                    if (hit.collider.GetComponent<PlayerController>() != null
                        || hit.collider.GetComponentInParent<PlayerController>() != null)
                    {
                        return col.transform;
                    }
                }
            }
            return null;
        }
    }
}
