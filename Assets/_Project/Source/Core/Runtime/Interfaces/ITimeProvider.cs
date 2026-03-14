namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Abstraction for time access. Enables deterministic testing
    /// and decouples logic from UnityEngine.Time.
    /// </summary>
    public interface ITimeProvider
    {
        float DeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float TimeSinceStartup { get; }
        float TimeScale { get; set; }
    }
}
