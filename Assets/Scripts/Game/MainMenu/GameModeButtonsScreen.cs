using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.MainMenu
{
    public class GameModeButtonsScreen : MonoBehaviour
    {
        public event Action<GameMode> OnRequestGameStart;
        public event Action<GameMode> OnRequestShowStatistics;
        public event Action<GameMode> OnRequestUnlockGameMode;
        public event Action OnRequestShowOverallStatistics;
        
        private enum ButtonMode
        {
            Play,
            Statistics
        }
        
        private const string PLAY_BUTTONS_LABEL = "graj";
        private const string STATISTICS_BUTTONS_LABEL = "statystyki";

        [SerializeField]
        private GameController _gameController;
        
        [SerializeField]
        private TextMeshProUGUI _buttonsLabel;
        
        [SerializeField]
        private Button _overallStatisticsButton;
        
        [SerializeField]
        private List<GameModeButton> _buttons = new List<GameModeButton>();
        
        private ButtonMode _currentButtonMode = ButtonMode.Play;

        public void Initialize()
        {
            _overallStatisticsButton.onClick.AddListener(HandleOverallStatisticsSelected);
            
            foreach (var button in _buttons)
            {
                button.OnGameModeSelected += HandleGameModeSelected;
                button.OnUnlockRequested += RequestUnlockGameMode;
                
                button.Initialize();
            }
        }

        private void HandleOverallStatisticsSelected()
        {
            OnRequestShowOverallStatistics.Invoke();
        }

        private void RequestUnlockGameMode(GameMode wordLength)
        {
            OnRequestUnlockGameMode.Invoke(wordLength);
        }

        private void HandleGameModeSelected(GameMode wordLength)
        {
            if (_currentButtonMode == ButtonMode.Play)
            {
                OnRequestGameStart.Invoke(wordLength);
            }
            else
            {
                OnRequestShowStatistics.Invoke(wordLength);
            }
        }

        public void ShowPlayButtons()
        {
            _overallStatisticsButton.gameObject.SetActive(false);
            _buttonsLabel.SetText(PLAY_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Play;
            
            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.UnlockedGameModes[gameModeIdx];
                button.gameObject.SetActive(true);
                button.RefreshAvailability(unlocked);
            }
        }

        public void ShowStatisticsButtons()
        {
            _overallStatisticsButton.gameObject.SetActive(true);
            _buttonsLabel.SetText(STATISTICS_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Statistics;

            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.UnlockedGameModes[gameModeIdx];
                button.RefreshAvailability(true);
                button.gameObject.SetActive(unlocked);
            }
        }

        public void RefreshAvailableGameModes()
        {
            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.UnlockedGameModes[gameModeIdx];
                button.RefreshAvailability(unlocked);
            }
        }
    }
}
