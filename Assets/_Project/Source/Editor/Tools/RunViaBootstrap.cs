#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using W1Style.Core.Constants;
using W1Style.Core.Launch;

namespace W1Style.Editor.Tools
{
    /// <summary>
    /// Editor menu item under W1Style/Run that launches the current scene through Bootstrap.
    /// Stores the active scene name as the target, opens Bootstrap, and enters play mode.
    /// BootstrapController reads the target from SessionState after domain reload.
    /// </summary>
    public static class RunViaBootstrap
    {
        private const string Tag = "[RunViaBootstrap]";
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string SessionKey = "W1Style.LaunchTarget";

        [MenuItem("W1Style/Run/Run Current Scene via Bootstrap", priority = 50)]
        public static void RunCurrentScene()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;

            if (string.IsNullOrWhiteSpace(currentSceneName))
            {
                UnityEngine.Debug.LogError($"{Tag} No active scene detected.");
                return;
            }

            if (currentSceneName == SceneNames.Bootstrap)
            {
                UnityEngine.Debug.LogWarning($"{Tag} Already in Bootstrap scene. Just entering play mode.");
                EditorApplication.isPlaying = true;
                return;
            }

            UnityEngine.Debug.Log($"{Tag} Launching scene '{currentSceneName}' via Bootstrap...");

            // Store target in static holder (survives within same domain)
            GameLaunchRequest.SetTargetScene(currentSceneName);

            // Also persist in SessionState (survives domain reload on play mode entry)
            SessionState.SetString(SessionKey, currentSceneName);

            // Save current scene, open Bootstrap, and enter play mode
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(BootstrapScenePath);
                EditorApplication.isPlaying = true;
            }
        }
    }
}
#endif
