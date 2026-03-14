using UnityEngine;
using W1Style.Core.Constants;
using W1Style.Core.Events;
using W1Style.Core.Interfaces;
using Zenject;

namespace W1Style.Bootstrap
{
    /// <summary>
    /// Startup controller that runs in the Bootstrap scene.
    /// Responsible for initializing the game and transitioning to the first real scene.
    ///
    /// Bootstrap flow:
    /// 1. Bootstrap scene is loaded (set as Scene 0 in Build Settings).
    /// 2. ProjectContext initializes (ProjectInstaller + ConfigInstaller).
    /// 3. SceneContext in Bootstrap scene creates BootstrapController.
    /// 4. BootstrapController.Start() runs startup sequence.
    /// 5. Transitions to MainMenu scene via ISceneService.
    /// </summary>
    public sealed class BootstrapController : MonoBehaviour
    {
        [Inject] private ILogService _log;
        [Inject] private ISceneService _sceneService;
        [Inject] private IEventBus _eventBus;

        private void Start()
        {
            _log.Info("Bootstrap started. Initializing game...");

            RunStartupSequence();
        }

        private void RunStartupSequence()
        {
            _log.Info("Running startup sequence...");

            _eventBus.Publish(new GameStateChangedEvent
            {
                PreviousState = GameState.None,
                NewState = GameState.Bootstrap
            });

            // Add any one-time initialization here:
            // - Load persistent data
            // - Initialize analytics
            // - Validate config integrity
            // - Warm up object pools

            TransitionToMainMenu();
        }

        private void TransitionToMainMenu()
        {
            _log.Info("Startup complete. Loading MainMenu...");

            _eventBus.Publish(new GameStateChangedEvent
            {
                PreviousState = GameState.Bootstrap,
                NewState = GameState.MainMenu
            });

            _sceneService.LoadSceneAsync(SceneNames.MainMenu);
        }
    }
}
