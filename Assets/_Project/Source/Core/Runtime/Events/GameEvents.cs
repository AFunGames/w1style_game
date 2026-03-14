namespace W1Style.Core.Events
{
    /// <summary>
    /// Shared event definitions used across features via the IEventBus.
    /// Add new structs here as the project grows.
    /// Keep events as plain data carriers with no logic.
    /// </summary>
    public struct SceneLoadedEvent
    {
        public string SceneName;
    }

    public struct GamePausedEvent
    {
        public bool IsPaused;
    }

    public struct GameStateChangedEvent
    {
        public GameState PreviousState;
        public GameState NewState;
    }

    public enum GameState
    {
        None,
        Bootstrap,
        MainMenu,
        Gameplay,
        Paused
    }
}
