using System;
using System.Collections.Generic;
using Sufka.GameFlow;
using Sufka.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Controls
{
    public class Keyboard : MonoBehaviour
    {
        public event Action<char> OnKeyPress;
        public event Action OnEnterPress;
        public event Action OnBackPress;
        public event Action OnHintPress;

        [SerializeField]
        private KeyGrid _keyGrid;
        
        [SerializeField]
        private Button _enterButton;
        
        [SerializeField]
        private Button _backButton;
        
        [SerializeField]
        private Button _hintButton;

        [SerializeField]
        private KeyboardLayout _keyboardLayout;

        [SerializeField]
        private GameController _gameController;
        

        private Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();

        public void Initialize()
        {
            _hintButton.interactable = _gameController.AvailableHints > 0;
            
            _enterButton.interactable = false;
            
            _enterButton.onClick.AddListener(HandleEnterPress);
            _backButton.onClick.AddListener(HandleBackPress);
            _hintButton.onClick.AddListener(HandleHintPress);
            _keyGrid.OnKeyPress += HandleKeyPress;

            _keyGrid.Initialize(_keyboardLayout.Keys);
        }

        private void HandleHintPress()
        {
            OnHintPress.Invoke();
            _hintButton.interactable = false;
        }

        private void HandleBackPress()
        {
            OnBackPress.Invoke();
        }

        private void HandleKeyPress(char letter)
        {
            OnKeyPress.Invoke(letter);
        }

        private void HandleEnterPress()
        {
            OnEnterPress.Invoke();
        }

        public void Refresh(ValidationResult result)
        {
            _keyGrid.Refresh(result);
        }

        public void Reset()
        {
            _keyGrid.Reset();
            _hintButton.interactable = _gameController.AvailableHints > 0;
            _enterButton.interactable = false;
        }

        public void EnableEnterButton()
        {
            _enterButton.interactable = true;
        }

        public void DisableEnterButton()
        {
            _enterButton.interactable = false;
        }

        public void MarkGuessed(char hintLetter)
        {
            _keyGrid.MarkGuessed(hintLetter);
        }
    }
}
