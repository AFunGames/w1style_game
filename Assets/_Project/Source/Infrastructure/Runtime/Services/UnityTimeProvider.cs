using UnityEngine;
using W1Style.Core.Interfaces;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Default time provider wrapping Unity's Time API.
    /// Injected where time-dependent logic needs to be testable.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class UnityTimeProvider : ITimeProvider
    {
        public float DeltaTime => Time.deltaTime;
        public float UnscaledDeltaTime => Time.unscaledDeltaTime;
        public float TimeSinceStartup => Time.realtimeSinceStartup;

        public float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}
