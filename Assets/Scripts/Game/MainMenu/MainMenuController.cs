using System;
using System.Collections.Generic;
using Sufka.GameFlow;
using Sufka.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sufka.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        private const string PLAY_BUTTONS_LABEL = "graj";
        private const string STATISTICS_BUTTONS_LABEL = "statystyki";
        
        private enum ButtonMode
        {
            Play,
            Statistics
        }

        public event Action<WordLength> OnRequestGameStart;

        [SerializeField]
        private GameObject _titleScreen;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _statisticsButton;

        [SerializeField]
        private Button _backButton;

        [FormerlySerializedAs("_playScreen"), SerializeField]
        private GameObject _buttonsScreen;

        [SerializeField]
        private StatisticsScreen _statisticsScreen;

        [SerializeField]
        private TextMeshProUGUI _buttonsLabel;

        [SerializeField]
        private List<WordLengthButton> _startGameButtons = new List<WordLengthButton>();

        private readonly Stack<GameObject> _backStack = new Stack<GameObject>();

        private ButtonMode _currentButtonMode = ButtonMode.Play;

        private void Start()
        {
            _playButton.onClick.AddListener(ShowPlayButtons);
            _backButton.onClick.AddListener(Back);
            _statisticsButton.onClick.AddListener(ShowStatisticsButtons);

            foreach (var button in _startGameButtons)
            {
                button.OnClick += HandleButtonClick;
            }

            _backStack.Push(_titleScreen);
        }

        private void ShowPlayButtons()
        {
            _buttonsLabel.SetText(PLAY_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Play;
            ShowScreen(_buttonsScreen);
        }

        private void ShowStatisticsButtons()
        {
            _buttonsLabel.SetText(STATISTICS_BUTTONS_LABEL);
            _currentButtonMode = ButtonMode.Statistics;
            ShowScreen(_buttonsScreen);
        }

        private void ShowScreen(GameObject screen)
        {
            _backStack.Peek().SetActive(false);
            screen.SetActive(true);
            _backStack.Push(screen);

            _backButton.gameObject.SetActive(true);
        }

        private void Back()
        {
            _backStack.Pop().SetActive(false);
            _backStack.Peek().SetActive(true);

            if (_backStack.Count <= 1)
            {
                _backButton.gameObject.SetActive(false);
            }
        }

        private void HandleButtonClick(WordLength wordLength)
        {
            if (_currentButtonMode == ButtonMode.Play)
            {
                ResetToTitleScreen();
                OnRequestGameStart.Invoke(wordLength);
            }
            else
            {
                ShowScreen(_statisticsScreen.gameObject);
                _statisticsScreen.Refresh(wordLength);
            }
        }

        private void ResetToTitleScreen()
        {
            while (_backStack.Count > 1)
            {
                _backStack.Pop().SetActive(false);
            }

            _backStack.Peek().SetActive(false);
            _backButton.gameObject.SetActive(false);
        }
    }
}
