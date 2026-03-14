#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace W1Style.Editor.Tools
{
    /// <summary>
    /// Utility menu items for project management.
    /// Provides quick actions for common development tasks.
    /// </summary>
    public static class ProjectMenuItems
    {
        [MenuItem("W1Style/Open/Project Configs Folder", priority = 300)]
        public static void OpenConfigsFolder()
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/_Project/Configs");
            if (obj != null)
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }
        }

        [MenuItem("W1Style/Open/Scenes Folder", priority = 301)]
        public static void OpenScenesFolder()
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/Scenes");
            if (obj != null)
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }
        }

        [MenuItem("W1Style/Clear/PlayerPrefs", priority = 400)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("[W1Style] PlayerPrefs cleared.");
        }
    }
}
#endif
