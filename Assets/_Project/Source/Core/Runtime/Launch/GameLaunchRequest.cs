namespace W1Style.Core.Launch
{
    /// <summary>
    /// Lightweight static state holder for scene launch requests.
    /// Used to pass the target scene name from any caller to the Bootstrap flow.
    ///
    /// Static fields persist within a single application session (play mode or build).
    /// No Unity dependencies — lives in W1Style.Core (noEngineReferences: true).
    /// </summary>
    public static class GameLaunchRequest
    {
        private static string _targetSceneName;

        /// <summary>
        /// Stores the scene to load after Bootstrap completes.
        /// Returns true if the value was accepted, false if null/empty.
        /// </summary>
        public static bool SetTargetScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
                return false;

            _targetSceneName = sceneName;
            return true;
        }

        /// <summary>
        /// Attempts to retrieve the stored target scene name.
        /// Returns true if a target was set, false otherwise.
        /// </summary>
        public static bool TryGetTargetScene(out string sceneName)
        {
            sceneName = _targetSceneName;
            return !string.IsNullOrWhiteSpace(_targetSceneName);
        }

        /// <summary>
        /// Clears the stored target scene. Call after the request has been consumed.
        /// </summary>
        public static void Clear()
        {
            _targetSceneName = null;
        }
    }
}
