using System;
using NUnit.Framework;
using W1Style.Core.Constants;
using W1Style.Core.Interfaces;
using W1Style.Core.Launch;
using W1Style.Infrastructure.Services;

namespace W1Style.Tests.EditMode.Infrastructure
{
    /// <summary>
    /// Unit tests for GameSceneLauncher service.
    /// Uses fake implementations of dependencies to test logic in isolation.
    /// </summary>
    [TestFixture]
    public sealed class GameSceneLauncherTests
    {
        private FakeSceneService _sceneService;
        private FakeAppLifecycleService _lifecycleService;
        private FakeLogService _logService;
        private GameSceneLauncher _launcher;

        [SetUp]
        public void SetUp()
        {
            GameLaunchRequest.Clear();
            _sceneService = new FakeSceneService();
            _lifecycleService = new FakeAppLifecycleService();
            _logService = new FakeLogService();
            _launcher = new GameSceneLauncher(_sceneService, _lifecycleService, _logService);
        }

        [TearDown]
        public void TearDown()
        {
            GameLaunchRequest.Clear();
        }

        [Test]
        public void LaunchViaBootstrap_WithNullScene_LogsError()
        {
            _launcher.LaunchViaBootstrap(null);

            Assert.IsTrue(_logService.LastError.Contains("null or empty"));
            Assert.IsNull(_sceneService.LastLoadedScene);
        }

        [Test]
        public void LaunchViaBootstrap_WithEmptyScene_LogsError()
        {
            _launcher.LaunchViaBootstrap("");

            Assert.IsTrue(_logService.LastError.Contains("null or empty"));
            Assert.IsNull(_sceneService.LastLoadedScene);
        }

        [Test]
        public void LaunchViaBootstrap_WithBootstrapAsTarget_LogsError()
        {
            _launcher.LaunchViaBootstrap(SceneNames.Bootstrap);

            Assert.IsTrue(_logService.LastError.Contains("infinite loop"));
            Assert.IsNull(_sceneService.LastLoadedScene);
        }

        [Test]
        public void LaunchCurrentSceneViaBootstrap_WhenInBootstrap_LogsWarning()
        {
            _sceneService.CurrentSceneName = SceneNames.Bootstrap;

            _launcher.LaunchCurrentSceneViaBootstrap();

            Assert.IsTrue(_logService.LastWarning.Contains("Already in Bootstrap"));
        }

        [Test]
        public void QuitGame_DelegatesToLifecycleService()
        {
            _launcher.QuitGame();

            Assert.IsTrue(_lifecycleService.QuitRequested);
        }

        #region Fakes

        private sealed class FakeSceneService : ISceneService
        {
            public string CurrentSceneName = "TestScene";
            public string LastLoadedScene;

            public string ActiveSceneName => CurrentSceneName;

            public void LoadScene(string sceneName)
            {
                LastLoadedScene = sceneName;
            }

            public void LoadSceneAsync(string sceneName, Action onComplete = null)
            {
                LastLoadedScene = sceneName;
                onComplete?.Invoke();
            }

            public void LoadSceneAdditive(string sceneName, Action onComplete = null)
            {
                LastLoadedScene = sceneName;
                onComplete?.Invoke();
            }

            public void UnloadScene(string sceneName, Action onComplete = null)
            {
                onComplete?.Invoke();
            }
        }

        private sealed class FakeAppLifecycleService : IAppLifecycleService
        {
            public bool QuitRequested;

            public event Action OnApplicationPausing;
            public event Action OnApplicationResuming;
            public event Action OnApplicationQuitting;

            public bool IsPaused => false;

            public void RequestQuit()
            {
                QuitRequested = true;
            }
        }

        private sealed class FakeLogService : ILogService
        {
            public string LastInfo;
            public string LastWarning;
            public string LastError;

            public void Info(string message) => LastInfo = message;
            public void Warning(string message) => LastWarning = message;
            public void Error(string message) => LastError = message;
        }

        #endregion
    }
}
