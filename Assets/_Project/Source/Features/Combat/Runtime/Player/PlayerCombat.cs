using UnityEngine;
using UnityEngine.InputSystem;
using W1Style.Features.Combat.Components;
using W1Style.Features.Combat.Domain;
using W1Style.Features.Combat.Noise;

namespace W1Style.Features.Combat.Player
{
    /// <summary>
    /// Handles melee combat actions: light attack, heavy attack, and kick.
    /// Uses sphere-casts from the camera to detect hits.
    /// </summary>
    public sealed class PlayerCombat : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform _attackOrigin;
        [SerializeField] private LayerMask _hitLayers = ~0;

        [Header("Light Attack")]
        [SerializeField] private float _lightDamage = 15f;
        [SerializeField] private float _lightRange = 2f;
        [SerializeField] private float _lightRadius = 0.3f;
        [SerializeField] private float _lightImpulse = 3f;
        [SerializeField] private float _lightCooldown = 0.4f;

        [Header("Heavy Attack")]
        [SerializeField] private float _heavyDamage = 40f;
        [SerializeField] private float _heavyRange = 2.5f;
        [SerializeField] private float _heavyRadius = 0.5f;
        [SerializeField] private float _heavyImpulse = 12f;
        [SerializeField] private float _heavyCooldown = 1f;

        [Header("Kick")]
        [SerializeField] private float _kickDamage = 5f;
        [SerializeField] private float _kickRange = 1.8f;
        [SerializeField] private float _kickRadius = 0.4f;
        [SerializeField] private float _kickImpulse = 18f;
        [SerializeField] private float _kickCooldown = 0.6f;

        [Header("Noise")]
        [SerializeField] private float _combatNoiseRadius = 15f;

        private float _cooldownTimer;

        private void Start()
        {
            if (_attackOrigin == null)
                _attackOrigin = GetComponentInChildren<Camera>().transform;
        }

        public void OnAttackLight(InputValue value)
        {
            if (value.isPressed)
                TryAttack(_lightDamage, _lightRange, _lightRadius, _lightImpulse, _lightCooldown);
        }

        public void OnAttackHeavy(InputValue value)
        {
            if (value.isPressed)
                TryAttack(_heavyDamage, _heavyRange, _heavyRadius, _heavyImpulse, _heavyCooldown);
        }

        public void OnKick(InputValue value)
        {
            if (value.isPressed)
                TryAttack(_kickDamage, _kickRange, _kickRadius, _kickImpulse, _kickCooldown);
        }

        private void Update()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
        }

        private void TryAttack(float damage, float range, float radius, float impulse, float cooldown)
        {
            if (_cooldownTimer > 0f) return;
            _cooldownTimer = cooldown;

            Vector3 origin = _attackOrigin.position;
            Vector3 direction = _attackOrigin.forward;

            if (!UnityEngine.Physics.SphereCast(origin, radius, direction, out RaycastHit hit, range, _hitLayers))
                return;

            // Determine if headshot via HitZone
            bool isHeadshot = false;
            var hitZone = hit.collider.GetComponent<HitZone>();
            if (hitZone != null)
                isHeadshot = hitZone.IsHead;

            var info = new DamageInfo
            {
                Amount = damage,
                HitPoint = hit.point,
                HitDirection = direction,
                ImpulseForce = impulse,
                Source = gameObject,
                IsHeadshot = isHeadshot
            };

            var damageable = hit.collider.GetComponentInParent<IDamageable>() as Component;
            if (damageable == null)
                damageable = hit.collider.GetComponent<Component>();

            if (damageable != null)
            {
                var dmg = damageable.GetComponentInParent<Damageable>();
                if (dmg != null)
                    dmg.TakeDamage(info);
            }

            // Apply physics impulse to anything with a rigidbody
            var rb = hit.collider.attachedRigidbody;
            if (rb != null)
                rb.AddForce(direction * impulse, ForceMode.Impulse);

            NoiseEmitter.EmitNoise(hit.point, _combatNoiseRadius);
        }
    }
}
