using UnityEngine;

namespace W1Style.Features.Combat.Domain
{
    /// <summary>
    /// Interface for any object that can receive damage.
    /// Implemented by <see cref="Components.Damageable"/>.
    /// </summary>
    public interface IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsDead { get; }
        void TakeDamage(DamageInfo info);
    }
}
