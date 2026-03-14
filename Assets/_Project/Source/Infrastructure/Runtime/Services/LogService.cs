using UnityEngine;
using W1Style.Core.Interfaces;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Default logging implementation using Unity's Debug API.
    /// Can be replaced with a structured logger or disabled in release builds.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class LogService : ILogService
    {
        private const string Prefix = "[W1Style]";

        public void Info(string message)
        {
            Debug.Log($"{Prefix} {message}");
        }

        public void Warning(string message)
        {
            Debug.LogWarning($"{Prefix} {message}");
        }

        public void Error(string message)
        {
            Debug.LogError($"{Prefix} {message}");
        }
    }
}
