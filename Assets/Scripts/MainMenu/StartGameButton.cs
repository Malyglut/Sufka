using System;
using Sufka.GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.MainMenu
{
    public class StartGameButton : MonoBehaviour
    {
        public event Action<WordLength> OnGameModeChosen;
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private WordLength _wordLength;

        private void Start()
        {
            _button.onClick.AddListener(ChooseGameMode);
        }

        private void ChooseGameMode()
        {
            OnGameModeChosen.Invoke(_wordLength);
        }
    }
}
