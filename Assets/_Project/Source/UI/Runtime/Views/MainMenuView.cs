using UnityEngine;
using UnityEngine.UI;
using W1Style.Core.Constants;
using W1Style.Core.Interfaces;
using Zenject;

namespace W1Style.UI.Views
{
    /// <summary>
    /// Main Menu screen with buttons for starting gameplay and quitting.
    /// Uses IGameLauncher for scene transitions to avoid duplicated launch logic.
    /// Wire buttons via the Inspector.
    /// </summary>
    public sealed class MainMenuView : MonoBehaviour
    {
        private const string Tag = "[MainMenuView]";

        [Header("Buttons")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitButton;

        [Inject] private IGameLauncher _launcher;
        [Inject] private ILogService _log;

        private void Start()
        {
            if (_startGameButton != null)
                _startGameButton.onClick.AddListener(OnStartGameClicked);
            else
                _log.Warning($"{Tag} Start game button is not assigned in the Inspector.");

            if (_exitButton != null)
                _exitButton.onClick.AddListener(OnExitClicked);
            else
                _log.Warning($"{Tag} Exit button is not assigned in the Inspector.");
        }

        private void OnStartGameClicked()
        {
            _log.Info($"{Tag} 'Запустити пісочницю' clicked. Launching Gameplay via Bootstrap...");
            _launcher.LaunchViaBootstrap(SceneNames.Gameplay);
        }

        private void OnExitClicked()
        {
            _log.Info($"{Tag} 'Вихід' clicked. Quitting application...");
            _launcher.QuitGame();
        }

        private void OnDestroy()
        {
            if (_startGameButton != null)
                _startGameButton.onClick.RemoveListener(OnStartGameClicked);

            if (_exitButton != null)
                _exitButton.onClick.RemoveListener(OnExitClicked);
        }
    }
}
