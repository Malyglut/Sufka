using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using TMPro;
using UnityEngine;

namespace Sufka.Game.MainMenu
{
    public class GameModeButtonsScreen : MonoBehaviour
    {
        public event Action<WordLength> OnRequestGameStart;
        public event Action<WordLength> OnRequestShowStatistics;
        
        private enum ButtonMode
        {
            Play,
            Statistics
        }
        
        private const string PLAY_BUTTONS_LABEL = "graj";
        private const string STATISTICS_BUTTONS_LABEL = "statystyki";
        
        [SerializeField]
        private TextMeshProUGUI _buttonsLabel;
        
        [SerializeField]
        private List<WordLengthButton> _startGameButtons = new List<WordLengthButton>();
        
        private ButtonMode _currentButtonMode = ButtonMode.Play;

        public void Initialize()
        {
            foreach (var button in _startGameButtons)
            {
                button.OnClick += HandleButtonClick;
            }
        }
        
        private void HandleButtonClick(WordLength wordLength)
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
        }

        public void ShowStatisticsButtons()
        {
            _buttonsLabel.SetText(STATISTICS_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Statistics;
        }
    }
}
