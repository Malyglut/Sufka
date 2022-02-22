using System;
using System.Collections.Generic;
using Sufka.Game.Colors;
using Sufka.Game.GameFlow;
using Sufka.Game.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public event Action<GameMode> OnRequestGameStart;
        public event Action<ColorScheme> OnRequestUnlockColorScheme;
        public event Action<GameMode> OnRequestUnlockGameMode;
        public event Action<ColorScheme> OnNotifyColorSchemeChanged;
        public event Action OnRequestContinueGame;

        [SerializeField]
        private GameObject _titleScreen;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _statisticsButton;

        [SerializeField]
        private Button _backButton;
        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private GameModeButtonsScreen _buttonsScreen;

        [SerializeField]
        private StatisticsScreen _statisticsScreen;

        [SerializeField]
        private ColorsScreen _colorsScreen;

        [SerializeField]
        private Button _colorsButton;
        
        [SerializeField]
        private Button _exitButton;

        private readonly Stack<GameObject> _backStack = new Stack<GameObject>();

        public void Initialize()
        {
            _buttonsScreen.Initialize();
            _buttonsScreen.OnRequestGameStart += RequestGameStart;
            _buttonsScreen.OnRequestShowStatistics += ShowStatistics;
            _buttonsScreen.OnRequestUnlockGameMode += RequestUnlockGameMode;
            
            _playButton.onClick.AddListener(ShowPlayButtons);
            _backButton.onClick.AddListener(Back);
            _statisticsButton.onClick.AddListener(ShowStatisticsButtons);
            _colorsButton.onClick.AddListener(ShowColorsScreen);
            _continueButton.onClick.AddListener(RequestContinueGame);

            _colorsScreen.OnUnlockColorSchemeRequested += RequestUnlockColorScheme;
            _colorsScreen.OnColorSchemeChanged += NotifyColorSchemeChanged;
            _colorsScreen.Initialize();
            
            _exitButton.onClick.AddListener(ExitGame);

            _backStack.Push(_titleScreen);
        }

        private void RequestUnlockGameMode(GameMode gameMode)
        {
            OnRequestUnlockGameMode.Invoke(gameMode);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void ShowStatistics(GameMode gameMode)
        {
            ShowScreen(_statisticsScreen.gameObject);
            _statisticsScreen.Refresh(gameMode);
        }

        private void RequestGameStart(GameMode gameMode)
        {
            ResetToTitleScreen();
            OnRequestGameStart.Invoke(gameMode);
        }

        private void NotifyColorSchemeChanged(ColorScheme colorScheme)
        {
            OnNotifyColorSchemeChanged.Invoke(colorScheme);
        }

        private void RequestUnlockColorScheme(ColorScheme colorScheme)
        {
            OnRequestUnlockColorScheme.Invoke(colorScheme);
        }

        private void RequestContinueGame()
        {
            ResetToTitleScreen();
            OnRequestContinueGame.Invoke();
        }

        private void ShowColorsScreen()
        {
            ShowScreen(_colorsScreen.gameObject);
        }

        private void ShowPlayButtons()
        {
            _buttonsScreen.ShowPlayButtons();
            ShowScreen(_buttonsScreen.gameObject);
        }

        private void ShowStatisticsButtons()
        {
            _buttonsScreen.ShowStatisticsButtons();
            ShowScreen(_buttonsScreen.gameObject);
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

        private void ResetToTitleScreen()
        {
            while (_backStack.Count > 1)
            {
                _backStack.Pop().SetActive(false);
            }

            _backStack.Peek().SetActive(false);
            _backButton.gameObject.SetActive(false);
        }

        public void ShowTitleScreen(bool gameInProgress)
        {
            _continueButton.gameObject.SetActive(gameInProgress);
            _backStack.Peek().SetActive(true);
        }

        public void RefreshColorSchemes()
        {
            _colorsScreen.RefreshAvailableColorSchemes();
        }
    }
}
