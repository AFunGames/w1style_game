using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Features.Combat.Config
{
    /// <summary>
    /// ScriptableObject configuration for combat system parameters.
    /// All gameplay values are exposed for Inspector tuning.
    /// </summary>
    [CreateAssetMenu(fileName = "CombatConfig", menuName = "W1Style/Features/Combat Config")]
    public sealed class CombatConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Player Movement")]
#endif
        [Header("Player Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _crouchSpeed = 2.5f;
        [SerializeField] private float _jumpForce = 6f;

#if ODIN_INSPECTOR
        [Title("Light Attack")]
#endif
        [Header("Light Attack")]
        [SerializeField] private float _lightDamage = 15f;
        [SerializeField] private float _lightRange = 2f;
        [SerializeField] private float _lightImpulse = 3f;
        [SerializeField] private float _lightCooldown = 0.4f;

#if ODIN_INSPECTOR
        [Title("Heavy Attack")]
#endif
        [Header("Heavy Attack")]
        [SerializeField] private float _heavyDamage = 40f;
        [SerializeField] private float _heavyRange = 2.5f;
        [SerializeField] private float _heavyImpulse = 12f;
        [SerializeField] private float _heavyCooldown = 1f;

#if ODIN_INSPECTOR
        [Title("Kick")]
#endif
        [Header("Kick")]
        [SerializeField] private float _kickDamage = 5f;
        [SerializeField] private float _kickRange = 1.8f;
        [SerializeField] private float _kickImpulse = 18f;
        [SerializeField] private float _kickCooldown = 0.6f;

#if ODIN_INSPECTOR
        [Title("Enemy")]
#endif
        [Header("Enemy")]
        [SerializeField] private float _enemyHealth = 50f;
        [SerializeField] private float _armoredEnemyHealth = 120f;
        [SerializeField] private float _knockdownForceThreshold = 10f;
        [SerializeField] private float _unconsciousTime = 4f;

#if ODIN_INSPECTOR
        [Title("Throw")]
#endif
        [Header("Throw")]
        [SerializeField] private float _throwForce = 20f;
        [SerializeField] private float _throwDamage = 15f;

#if ODIN_INSPECTOR
        [Title("Noise")]
#endif
        [Header("Noise")]
        [SerializeField] private float _combatNoiseRadius = 15f;
        [SerializeField] private float _sprintNoiseRadius = 12f;
        [SerializeField] private float _throwNoiseRadius = 10f;

        // Accessors
        public float MoveSpeed => _moveSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float CrouchSpeed => _crouchSpeed;
        public float JumpForce => _jumpForce;

        public float LightDamage => _lightDamage;
        public float LightRange => _lightRange;
        public float LightImpulse => _lightImpulse;
        public float LightCooldown => _lightCooldown;

        public float HeavyDamage => _heavyDamage;
        public float HeavyRange => _heavyRange;
        public float HeavyImpulse => _heavyImpulse;
        public float HeavyCooldown => _heavyCooldown;

        public float KickDamage => _kickDamage;
        public float KickRange => _kickRange;
        public float KickImpulse => _kickImpulse;
        public float KickCooldown => _kickCooldown;

        public float EnemyHealth => _enemyHealth;
        public float ArmoredEnemyHealth => _armoredEnemyHealth;
        public float KnockdownForceThreshold => _knockdownForceThreshold;
        public float UnconsciousTime => _unconsciousTime;

        public float ThrowForce => _throwForce;
        public float ThrowDamage => _throwDamage;

        public float CombatNoiseRadius => _combatNoiseRadius;
        public float SprintNoiseRadius => _sprintNoiseRadius;
        public float ThrowNoiseRadius => _throwNoiseRadius;
    }
}
