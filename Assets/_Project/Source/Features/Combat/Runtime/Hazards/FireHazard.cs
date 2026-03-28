using UnityEngine;
using W1Style.Features.Combat.Components;

namespace W1Style.Features.Combat.Hazards
{
    /// <summary>
    /// Fire hazard. Applies continuous damage to entities inside the zone.
    /// Enemies pushed into fire take damage each tick.
    /// </summary>
    public sealed class FireHazard : HazardZone
    {
        [Header("Fire")]
        [SerializeField] private float _fireDamagePerSecond = 50f;

        private void OnTriggerStay(Collider other)
        {
            var damageable = other.GetComponentInParent<Damageable>();
            if (damageable == null || damageable.IsDead) return;

            var info = new Domain.DamageInfo
            {
                Amount = _fireDamagePerSecond * Time.deltaTime,
                HitPoint = other.transform.position,
                HitDirection = Vector3.up,
                ImpulseForce = 0f,
                Source = gameObject,
                IsHeadshot = false
            };
            damageable.TakeDamage(info);
        }
    }
}
