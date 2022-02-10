using System;
using System.Collections.Generic;
using Sufka.GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public event Action<WordLength> OnRequestGameStart;
        
        [SerializeField]
        private GameObject _titleScreen;

        [SerializeField]
        private Button _playButton;
        
        [SerializeField]
        private Button _statisticsButton;

        [SerializeField]
        private GameObject _playScreen;
        
        [SerializeField]
        private GameObject _statisticsScreen;

        [SerializeField]
        private List<StartGameButton> _startGameButtons = new List<StartGameButton>();

        private Stack<GameObject> _backStack = new Stack<GameObject>();

        private void Start()
        {
            _playButton.onClick.AddListener(ShowPlayScreen);
            // _statisticsButton.onClick.AddListener(null);

            foreach (var button in _startGameButtons)
            {
                button.OnGameModeChosen += RequestGameStart;
            }
            
            _backStack.Push(_titleScreen);
        }

        private void ShowPlayScreen()
        {
            ShowScreen(_playScreen);
        }

        private void ShowScreen(GameObject screen)
        {
            _backStack.Peek().SetActive(false);
            screen.SetActive(true);
            _backStack.Push(screen);
        }

        private void Back()
        {
            _backStack.Pop().SetActive(false);
            _backStack.Peek().SetActive(true);
        }

        private void RequestGameStart(WordLength wordLength)
        {
            while (_backStack.Count>1)
            {
                _backStack.Pop().SetActive(false);
            }
            
            _backStack.Peek().SetActive(false);
            
            OnRequestGameStart.Invoke(wordLength);
        }
    }
}
