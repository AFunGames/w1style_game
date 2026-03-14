using UnityEngine;
using W1Style.Core.Constants;
using W1Style.Core.Events;
using W1Style.Core.Interfaces;
using W1Style.Core.Launch;
using Zenject;

namespace W1Style.Bootstrap
{
    /// <summary>
    /// Startup controller that runs in the Bootstrap scene.
    /// Responsible for initializing the game and transitioning to the target scene.
    ///
    /// Bootstrap flow:
    /// 1. Bootstrap scene is loaded (set as Scene 0 in Build Settings).
    /// 2. ProjectContext initializes (ProjectInstaller + ConfigInstaller).
    /// 3. SceneContext in Bootstrap scene creates BootstrapController.
    /// 4. BootstrapController.Start() runs startup sequence.
    /// 5. Reads GameLaunchRequest for a target scene.
    /// 6. Transitions to the requested scene, or MainMenu as fallback.
    /// </summary>
    public sealed class BootstrapController : MonoBehaviour
    {
        private const string Tag = "[BootstrapController]";
        private const string EditorSessionKey = "W1Style.LaunchTarget";

        [Inject] private ILogService _log;
        [Inject] private ISceneService _sceneService;
        [Inject] private IEventBus _eventBus;

        private void Start()
        {
            _log.Info($"{Tag} Bootstrap started. Initializing game...");

            RunStartupSequence();
        }

        private void RunStartupSequence()
        {
            _log.Info($"{Tag} Running startup sequence...");

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

            TransitionToTargetScene();
        }

        private void TransitionToTargetScene()
        {
            var targetScene = ResolveTargetScene();

            if (targetScene == SceneNames.Bootstrap)
            {
                _log.Error($"{Tag} Target scene is Bootstrap — refusing to reload to avoid infinite loop. Falling back to MainMenu.");
                targetScene = SceneNames.MainMenu;
            }

            _log.Info($"{Tag} Startup complete. Loading '{targetScene}'...");

            var newState = targetScene == SceneNames.MainMenu
                ? GameState.MainMenu
                : GameState.Gameplay;

            _eventBus.Publish(new GameStateChangedEvent
            {
                PreviousState = GameState.Bootstrap,
                NewState = newState
            });

            _sceneService.LoadSceneAsync(targetScene);
        }

        private string ResolveTargetScene()
        {
            // 1. Check static launch request (set at runtime by GameSceneLauncher)
            if (GameLaunchRequest.TryGetTargetScene(out var requested))
            {
                _log.Info($"{Tag} Launch request found: '{requested}'.");
                GameLaunchRequest.Clear();
                return requested;
            }

#if UNITY_EDITOR
            // 2. In editor, check SessionState (survives domain reload from RunViaBootstrap)
            var editorTarget = UnityEditor.SessionState.GetString(EditorSessionKey, null);
            if (!string.IsNullOrWhiteSpace(editorTarget))
            {
                UnityEditor.SessionState.EraseString(EditorSessionKey);
                _log.Info($"{Tag} Editor launch target found: '{editorTarget}'.");
                return editorTarget;
            }
#endif

            // 3. No request — fallback to MainMenu
            _log.Info($"{Tag} No launch request. Falling back to MainMenu.");
            return SceneNames.MainMenu;
        }
    }
}
