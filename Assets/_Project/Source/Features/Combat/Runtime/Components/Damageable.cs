using System;
using UnityEngine;
using W1Style.Features.Combat.Domain;

namespace W1Style.Features.Combat.Components
{
    /// <summary>
    /// Health and damage component for any damageable object or character.
    /// Attach to enemies, destructible props, etc.
    /// Handles health tracking, headshot logic, and death.
    /// </summary>
    public sealed class Damageable : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float _maxHealth = 50f;

        [Header("Headshot")]
        [SerializeField] private bool _instantKillOnHeadshot = true;

        private float _currentHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;

        /// <summary>Raised when damage is received (not dead yet).</summary>
        public event Action<DamageInfo> OnDamaged;

        /// <summary>Raised when health reaches zero.</summary>
        public event Action OnDied;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (IsDead) return;

            if (info.IsHeadshot && _instantKillOnHeadshot)
            {
                _currentHealth = 0f;
            }
            else
            {
                _currentHealth = Mathf.Max(0f, _currentHealth - info.Amount);
            }

            OnDamaged?.Invoke(info);

            if (IsDead)
                OnDied?.Invoke();
        }

        /// <summary>
        /// Instantly kills this entity. Used by hazard zones.
        /// </summary>
        public void InstantKill()
        {
            if (IsDead) return;
            _currentHealth = 0f;
            OnDamaged?.Invoke(default);
            OnDied?.Invoke();
        }
    }
}
