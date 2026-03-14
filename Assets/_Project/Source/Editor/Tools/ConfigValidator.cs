#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using W1Style.Configs;

namespace W1Style.Editor.Tools
{
    /// <summary>
    /// Validates that the root GameConfig asset has all required sub-configs assigned.
    /// Run from the editor menu or as part of a build validation step.
    /// </summary>
    public static class ConfigValidator
    {
        [MenuItem("W1Style/Validation/Validate Game Config", priority = 200)]
        public static void ValidateGameConfig()
        {
            var guids = AssetDatabase.FindAssets("t:GameConfig");
            if (guids.Length == 0)
            {
                Debug.LogError("[ConfigValidator] No GameConfig asset found. Create one via W1Style/Create/Game Config.");
                return;
            }

            var valid = true;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<GameConfig>(path);

                if (config.Audio == null)
                {
                    Debug.LogError($"[ConfigValidator] GameConfig at '{path}' is missing AudioConfig reference.");
                    valid = false;
                }

                if (config.Gameplay == null)
                {
                    Debug.LogError($"[ConfigValidator] GameConfig at '{path}' is missing GameplayConfig reference.");
                    valid = false;
                }

                if (config.UI == null)
                {
                    Debug.LogError($"[ConfigValidator] GameConfig at '{path}' is missing UIConfig reference.");
                    valid = false;
                }

                if (valid)
                {
                    Debug.Log($"[ConfigValidator] GameConfig at '{path}' is valid.");
                }
            }
        }
    }
}
#endif
