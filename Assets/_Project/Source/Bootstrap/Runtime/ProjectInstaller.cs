using W1Style.Core.Interfaces;
using W1Style.Infrastructure.Services;
using Zenject;

namespace W1Style.Bootstrap
{
    /// <summary>
    /// Global Zenject installer bound to the ProjectContext.
    /// Registers all core services that persist across scenes.
    ///
    /// Registered services:
    /// - ILogService (singleton)
    /// - IEventBus (singleton)
    /// - ITimeProvider (singleton)
    /// - ISceneService (singleton)
    /// - ISaveService (singleton)
    /// - IAppLifecycleService (singleton, MonoBehaviour on ProjectContext)
    /// - IGameLauncher (singleton)
    /// </summary>
    public sealed class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            // Logging
            Container.Bind<ILogService>()
                .To<LogService>()
                .AsSingle()
                .NonLazy();

            // Event Bus
            Container.Bind<IEventBus>()
                .To<EventBus>()
                .AsSingle();

            // Time
            Container.Bind<ITimeProvider>()
                .To<UnityTimeProvider>()
                .AsSingle();

            // Scene Loading
            Container.Bind<ISceneService>()
                .To<SceneService>()
                .AsSingle();

            // Save/Load
            Container.Bind<ISaveService>()
                .To<PlayerPrefsSaveService>()
                .AsSingle();

            // App Lifecycle (MonoBehaviour)
            Container.BindInterfacesTo<AppLifecycleService>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("AppLifecycleService")
                .AsSingle()
                .NonLazy();

            // Game Launcher
            Container.Bind<IGameLauncher>()
                .To<GameSceneLauncher>()
                .AsSingle();
        }
    }
}
