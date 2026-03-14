using UnityEngine;
using W1Style.Configs;
using Zenject;

namespace W1Style.Bootstrap
{
    /// <summary>
    /// Zenject installer that binds the root GameConfig and all subsystem configs.
    /// Attached to the ProjectContext so configs are available globally.
    ///
    /// The GameConfig ScriptableObject is assigned in the inspector on this installer.
    /// All sub-configs are extracted and bound individually for direct injection.
    /// </summary>
    public sealed class ConfigInstaller : MonoInstaller<ConfigInstaller>
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            // Bind root config
            Container.Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            // Bind sub-configs for direct injection
            Container.Bind<AudioConfig>()
                .FromInstance(_gameConfig.Audio)
                .AsSingle();

            Container.Bind<GameplayConfig>()
                .FromInstance(_gameConfig.Gameplay)
                .AsSingle();

            Container.Bind<UIConfig>()
                .FromInstance(_gameConfig.UI)
                .AsSingle();
        }
    }
}
