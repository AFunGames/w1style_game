using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Configs
{
    /// <summary>
    /// Root configuration asset that links all subsystem configs.
    /// Create one instance at Assets/_Project/Configs/Resources/GameConfig.asset.
    /// Referenced by ConfigInstaller to inject all configs into the DI container.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "W1Style/Configs/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Subsystem Configurations")]
        [Required("AudioConfig is required for the game to function.")]
#endif
        [Header("Audio")]
        [SerializeField] private AudioConfig _audioConfig;

#if ODIN_INSPECTOR
        [Required("GameplayConfig is required for the game to function.")]
#endif
        [Header("Gameplay")]
        [SerializeField] private GameplayConfig _gameplayConfig;

#if ODIN_INSPECTOR
        [Required("UIConfig is required for the game to function.")]
#endif
        [Header("UI")]
        [SerializeField] private UIConfig _uiConfig;

        public AudioConfig Audio => _audioConfig;
        public GameplayConfig Gameplay => _gameplayConfig;
        public UIConfig UI => _uiConfig;
    }
}
