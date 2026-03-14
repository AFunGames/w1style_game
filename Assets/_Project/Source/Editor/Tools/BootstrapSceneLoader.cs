#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace W1Style.Editor.Tools
{
    /// <summary>
    /// Editor menu item to quickly open the Bootstrap scene.
    /// Saves time during development by providing one-click access.
    /// </summary>
    public static class BootstrapSceneLoader
    {
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";

        [MenuItem("W1Style/Scenes/Open Bootstrap Scene", priority = 100)]
        public static void OpenBootstrapScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(BootstrapScenePath);
            }
        }

        [MenuItem("W1Style/Scenes/Open MainMenu Scene", priority = 101)]
        public static void OpenMainMenuScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Project/Scenes/MainMenu.unity");
            }
        }

        [MenuItem("W1Style/Scenes/Open Gameplay Scene", priority = 102)]
        public static void OpenGameplayScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Project/Scenes/Gameplay.unity");
            }
        }
    }
}
#endif
