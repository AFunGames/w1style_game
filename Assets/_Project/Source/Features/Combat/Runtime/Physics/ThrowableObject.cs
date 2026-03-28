using UnityEngine;
using W1Style.Features.Combat.Components;
using W1Style.Features.Combat.Domain;
using W1Style.Features.Combat.Noise;

namespace W1Style.Features.Combat.Physics
{
    /// <summary>
    /// A physics object the player can pick up and throw.
    /// When thrown, damages and stuns enemies on impact.
    /// Also generates noise on collision.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ThrowableObject : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private float _impactDamage = 15f;
        [SerializeField] private float _impactStunForce = 6f;
        [SerializeField] private float _minImpactSpeed = 3f;

        [Header("Noise")]
        [SerializeField] private float _impactNoiseRadius = 8f;

        private Rigidbody _rb;
        private Collider _collider;
        private bool _isHeld;
        private bool _wasThrown;
        private Transform _originalParent;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _originalParent = transform.parent;
        }

        /// <summary>
        /// Called when the player picks up this object.
        /// </summary>
        public void Pickup(Transform holdPoint)
        {
            _isHeld = true;
            _wasThrown = false;
            _rb.isKinematic = true;
            _collider.enabled = false;
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Called when the player throws this object.
        /// </summary>
        public void Throw(Vector3 force)
        {
            _isHeld = false;
            _wasThrown = true;
            transform.SetParent(_originalParent);
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_wasThrown) return;
            if (collision.relativeVelocity.magnitude < _minImpactSpeed) return;

            _wasThrown = false;

            // Damage enemy if hit
            var damageable = collision.collider.GetComponentInParent<Damageable>();
            if (damageable != null)
            {
                var info = new DamageInfo
                {
                    Amount = _impactDamage,
                    HitPoint = collision.GetContact(0).point,
                    HitDirection = collision.relativeVelocity.normalized,
                    ImpulseForce = _impactStunForce,
                    Source = gameObject,
                    IsHeadshot = false
                };
                damageable.TakeDamage(info);
            }

            // Apply stun force to rigidbody
            var targetRb = collision.collider.attachedRigidbody;
            if (targetRb != null && targetRb != _rb)
            {
                targetRb.AddForce(
                    collision.relativeVelocity.normalized * _impactStunForce,
                    ForceMode.Impulse);
            }

            NoiseEmitter.EmitNoise(transform.position, _impactNoiseRadius);
        }
    }
}
