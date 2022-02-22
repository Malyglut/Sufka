using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using TMPro;
using UnityEngine;

namespace Sufka.Game.MainMenu
{
    public class GameModeButtonsScreen : MonoBehaviour
    {
        public event Action<GameMode> OnRequestGameStart;
        public event Action<GameMode> OnRequestShowStatistics;
        public event Action<GameMode> OnRequestUnlockGameMode;
        
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
        private List<GameModeButton> _buttons = new List<GameModeButton>();
        
        private ButtonMode _currentButtonMode = ButtonMode.Play;

        public void Initialize()
        {
            foreach (var button in _buttons)
            {
                button.OnGameModeSelected += HandleGameModeSelected;
                button.OnUnlockRequested += RequestUnlockGameMode;
                
                button.Initialize();
            }
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
            _buttonsLabel.SetText(PLAY_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Play;
            
            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.Unlocks.unlockedGameModes[gameModeIdx];
                button.gameObject.SetActive(true);
                button.RefreshAvailability(unlocked);
            }
        }

        public void ShowStatisticsButtons()
        {
            _buttonsLabel.SetText(STATISTICS_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Statistics;

            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.Unlocks.unlockedGameModes[gameModeIdx];
                button.RefreshAvailability(true);
                button.gameObject.SetActive(unlocked);
            }
        }

        public void RefreshAvailableGameModes()
        {
            foreach (var button in _buttons)
            {
                var gameModeIdx = _gameController.GetGameModeIdx(button.GameMode);
                var unlocked = _gameController.Unlocks.unlockedGameModes[gameModeIdx];
                button.RefreshAvailability(unlocked);
            }
        }
    }
}
