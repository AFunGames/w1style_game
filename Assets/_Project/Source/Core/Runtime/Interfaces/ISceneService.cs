using System;

namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Abstraction for scene loading. Decouples gameplay code from
    /// UnityEngine.SceneManagement and supports async transitions.
    /// </summary>
    public interface ISceneService
    {
        void LoadScene(string sceneName);
        void LoadSceneAsync(string sceneName, Action onComplete = null);
        void LoadSceneAdditive(string sceneName, Action onComplete = null);
        void UnloadScene(string sceneName, Action onComplete = null);
        string ActiveSceneName { get; }
    }
}
