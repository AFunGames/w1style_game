using System;

namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Handles application lifecycle events (pause, focus, quit).
    /// Centralizes cleanup and state-saving on app transitions.
    /// </summary>
    public interface IAppLifecycleService
    {
        event Action OnApplicationPausing;
        event Action OnApplicationResuming;
        event Action OnApplicationQuitting;

        bool IsPaused { get; }
        void RequestQuit();
    }
}
