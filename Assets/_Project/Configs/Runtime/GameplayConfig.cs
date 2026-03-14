using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Configs
{
    /// <summary>
    /// Gameplay configuration values.
    /// Holds tuning parameters for core gameplay systems.
    /// </summary>
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "W1Style/Configs/Gameplay Config")]
    public sealed class GameplayConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Player Settings")]
#endif
        [Header("Player")]
        [SerializeField] private float _playerMoveSpeed = 5f;
        [SerializeField] private float _playerMaxHealth = 100f;

#if ODIN_INSPECTOR
        [Title("World Settings")]
#endif
        [Header("World")]
        [SerializeField] private float _gravityScale = 1f;
        [SerializeField] private int _maxEntitiesPerScene = 500;

        public float PlayerMoveSpeed => _playerMoveSpeed;
        public float PlayerMaxHealth => _playerMaxHealth;
        public float GravityScale => _gravityScale;
        public int MaxEntitiesPerScene => _maxEntitiesPerScene;
    }
}
