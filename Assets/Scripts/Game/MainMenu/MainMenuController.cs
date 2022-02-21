using System;
using System.Collections.Generic;
using Sufka.Game.Colors;
using Sufka.Game.GameFlow;
using Sufka.Game.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.MainMenu
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

        [SerializeField]
        private GameObject _buttonsScreen;

        [SerializeField]
        private StatisticsScreen _statisticsScreen;
        
        [SerializeField]
        private ColorsScreen _colorsScreen;

        [SerializeField]
        private TextMeshProUGUI _buttonsLabel;

        [SerializeField]
        private Button _colorsButton;

        [SerializeField]
        private List<WordLengthButton> _startGameButtons = new List<WordLengthButton>();

        private readonly Stack<GameObject> _backStack = new Stack<GameObject>();

        private ButtonMode _currentButtonMode = ButtonMode.Play;

        private void Start()
        {
            _playButton.onClick.AddListener(ShowPlayButtons);
            _backButton.onClick.AddListener(Back);
            _statisticsButton.onClick.AddListener(ShowStatisticsButtons);
            _colorsButton.onClick.AddListener(ShowColorsScreen);
            
            _colorsScreen.Initialize();

            foreach (var button in _startGameButtons)
            {
                button.OnClick += HandleButtonClick;
            }

            _backStack.Push(_titleScreen);
        }

        private void ShowColorsScreen()
        {
            ShowScreen(_colorsScreen.gameObject);
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

        public void ShowTitleScreen()
        {
            _backStack.Peek().SetActive(true);
        }
    }
}
