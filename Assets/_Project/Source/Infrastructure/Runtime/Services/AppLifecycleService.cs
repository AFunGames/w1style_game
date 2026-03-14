using System;
using UnityEngine;
using W1Style.Core.Interfaces;
using Zenject;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Handles Unity application lifecycle events (pause, focus, quit).
    /// Lives as a MonoBehaviour on the ProjectContext so it persists across scenes.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class AppLifecycleService : MonoBehaviour, IAppLifecycleService, IInitializable
    {
        public event Action OnApplicationPausing;
        public event Action OnApplicationResuming;
        public event Action OnApplicationQuitting;

        public bool IsPaused { get; private set; }

        [Inject] private ILogService _log;

        public void Initialize()
        {
            _log.Info("AppLifecycleService initialized.");
        }

        public void RequestQuit()
        {
            _log.Info("Application quit requested.");
            OnApplicationQuitting?.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            IsPaused = pauseStatus;
            if (pauseStatus)
            {
                _log.Info("Application paused.");
                OnApplicationPausing?.Invoke();
            }
            else
            {
                _log.Info("Application resumed.");
                OnApplicationResuming?.Invoke();
            }
        }

        private void OnApplicationQuit()
        {
            _log.Info("Application quitting.");
            OnApplicationQuitting?.Invoke();
        }
    }
}
