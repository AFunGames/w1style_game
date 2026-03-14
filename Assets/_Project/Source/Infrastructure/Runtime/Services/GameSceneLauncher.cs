using UnityEngine;
using UnityEngine.SceneManagement;
using W1Style.Core.Constants;
using W1Style.Core.Interfaces;
using W1Style.Core.Launch;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Centralized service for launching scenes through the Bootstrap flow.
    /// Validates scene names, stores the launch request, and triggers Bootstrap.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class GameSceneLauncher : IGameLauncher
    {
        private const string Tag = "[GameSceneLauncher]";

        private readonly ISceneService _sceneService;
        private readonly IAppLifecycleService _lifecycleService;
        private readonly ILogService _log;

        public GameSceneLauncher(
            ISceneService sceneService,
            IAppLifecycleService lifecycleService,
            ILogService log)
        {
            _sceneService = sceneService;
            _lifecycleService = lifecycleService;
            _log = log;
        }

        public void LaunchViaBootstrap(string targetSceneName)
        {
            if (string.IsNullOrWhiteSpace(targetSceneName))
            {
                _log.Error($"{Tag} Cannot launch: target scene name is null or empty.");
                return;
            }

            if (targetSceneName == SceneNames.Bootstrap)
            {
                _log.Error($"{Tag} Cannot launch: target scene cannot be Bootstrap (would cause infinite loop).");
                return;
            }

            if (!IsSceneInBuildSettings(targetSceneName))
            {
                _log.Error($"{Tag} Cannot launch: scene '{targetSceneName}' is not in Build Settings. Add it to File > Build Settings.");
                return;
            }

            _log.Info($"{Tag} Launch requested: target='{targetSceneName}'. Loading Bootstrap...");

            GameLaunchRequest.SetTargetScene(targetSceneName);
            _sceneService.LoadScene(SceneNames.Bootstrap);
        }

        public void LaunchCurrentSceneViaBootstrap()
        {
            var currentScene = _sceneService.ActiveSceneName;

            if (currentScene == SceneNames.Bootstrap)
            {
                _log.Warning($"{Tag} Already in Bootstrap scene. Ignoring launch request.");
                return;
            }

            _log.Info($"{Tag} Launching current scene '{currentScene}' via Bootstrap.");
            LaunchViaBootstrap(currentScene);
        }

        public void QuitGame()
        {
            _log.Info($"{Tag} Quit game requested.");
            _lifecycleService.RequestQuit();
        }

        /// <summary>
        /// Checks whether a scene with the given name exists in Build Settings.
        /// </summary>
        public static bool IsSceneInBuildSettings(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (name == sceneName)
                    return true;
            }

            return false;
        }
    }
}
