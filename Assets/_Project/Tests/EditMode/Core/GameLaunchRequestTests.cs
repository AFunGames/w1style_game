using NUnit.Framework;
using W1Style.Core.Launch;

namespace W1Style.Tests.EditMode.Core
{
    /// <summary>
    /// Unit tests for GameLaunchRequest static state holder.
    /// Pure C# tests — no Unity scene required.
    /// </summary>
    [TestFixture]
    public sealed class GameLaunchRequestTests
    {
        [SetUp]
        public void SetUp()
        {
            GameLaunchRequest.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            GameLaunchRequest.Clear();
        }

        [Test]
        public void SetTargetScene_WithValidName_ReturnsTrue()
        {
            var result = GameLaunchRequest.SetTargetScene("Gameplay");

            Assert.IsTrue(result);
        }

        [Test]
        public void TryGetTargetScene_AfterSet_ReturnsTrueWithCorrectName()
        {
            GameLaunchRequest.SetTargetScene("Gameplay");

            var result = GameLaunchRequest.TryGetTargetScene(out var sceneName);

            Assert.IsTrue(result);
            Assert.AreEqual("Gameplay", sceneName);
        }

        [Test]
        public void TryGetTargetScene_WithoutSet_ReturnsFalse()
        {
            var result = GameLaunchRequest.TryGetTargetScene(out var sceneName);

            Assert.IsFalse(result);
            Assert.IsTrue(string.IsNullOrWhiteSpace(sceneName));
        }

        [Test]
        public void Clear_RemovesStoredScene()
        {
            GameLaunchRequest.SetTargetScene("Gameplay");
            GameLaunchRequest.Clear();

            var result = GameLaunchRequest.TryGetTargetScene(out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void SetTargetScene_WithNull_ReturnsFalse()
        {
            var result = GameLaunchRequest.SetTargetScene(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void SetTargetScene_WithEmptyString_ReturnsFalse()
        {
            var result = GameLaunchRequest.SetTargetScene("");

            Assert.IsFalse(result);
        }

        [Test]
        public void SetTargetScene_WithWhitespace_ReturnsFalse()
        {
            var result = GameLaunchRequest.SetTargetScene("   ");

            Assert.IsFalse(result);
        }

        [Test]
        public void SetTargetScene_OverwritesPreviousValue()
        {
            GameLaunchRequest.SetTargetScene("MainMenu");
            GameLaunchRequest.SetTargetScene("Gameplay");

            GameLaunchRequest.TryGetTargetScene(out var sceneName);

            Assert.AreEqual("Gameplay", sceneName);
        }

        [Test]
        public void SetTargetScene_WithNull_DoesNotOverwriteExisting()
        {
            GameLaunchRequest.SetTargetScene("Gameplay");
            GameLaunchRequest.SetTargetScene(null);

            GameLaunchRequest.TryGetTargetScene(out var sceneName);

            Assert.AreEqual("Gameplay", sceneName);
        }
    }
}
