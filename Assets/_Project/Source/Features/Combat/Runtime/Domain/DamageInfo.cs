using UnityEngine;

namespace W1Style.Features.Combat.Domain
{
    /// <summary>
    /// Data carrier for damage events.
    /// Passed to <see cref="IDamageable.TakeDamage"/>.
    /// </summary>
    public struct DamageInfo
    {
        public float Amount;
        public Vector3 HitPoint;
        public Vector3 HitDirection;
        public float ImpulseForce;
        public GameObject Source;
        public bool IsHeadshot;
    }
}
