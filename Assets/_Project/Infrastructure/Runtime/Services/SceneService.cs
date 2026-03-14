using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using W1Style.Core.Interfaces;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Scene loading service wrapping Unity's SceneManager.
    /// Provides sync and async loading with callbacks.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class SceneService : ISceneService
    {
        private readonly ILogService _log;

        public SceneService(ILogService log)
        {
            _log = log;
        }

        public string ActiveSceneName => SceneManager.GetActiveScene().name;

        public void LoadScene(string sceneName)
        {
            _log.Info($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName, Action onComplete = null)
        {
            _log.Info($"Loading scene async: {sceneName}");
            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation != null && onComplete != null)
            {
                operation.completed += _ => onComplete.Invoke();
            }
        }

        public void LoadSceneAdditive(string sceneName, Action onComplete = null)
        {
            _log.Info($"Loading scene additive: {sceneName}");
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (operation != null && onComplete != null)
            {
                operation.completed += _ => onComplete.Invoke();
            }
        }

        public void UnloadScene(string sceneName, Action onComplete = null)
        {
            _log.Info($"Unloading scene: {sceneName}");
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            if (operation != null && onComplete != null)
            {
                operation.completed += _ => onComplete.Invoke();
            }
        }
    }
}
