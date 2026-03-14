namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Service for launching scenes through the Bootstrap flow.
    /// Provides a centralized, reusable entry point for scene transitions
    /// so UI buttons and editor tools don't duplicate logic.
    /// </summary>
    public interface IGameLauncher
    {
        /// <summary>
        /// Stores the target scene and loads Bootstrap.
        /// Validates that the scene name is not empty and not Bootstrap itself.
        /// </summary>
        void LaunchViaBootstrap(string targetSceneName);

        /// <summary>
        /// Detects the currently active scene and launches it through Bootstrap.
        /// Useful for restarting the current scene through the full startup flow.
        /// </summary>
        void LaunchCurrentSceneViaBootstrap();

        /// <summary>
        /// Quits the application. In the editor, logs a message and stops play mode.
        /// </summary>
        void QuitGame();
    }
}
