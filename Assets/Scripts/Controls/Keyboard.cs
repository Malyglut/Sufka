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
        private HintButton _hintButton;

        [SerializeField]
        private KeyboardLayout _keyboardLayout;

        [SerializeField]
        private ButtonColors _enterButtonColors;

        private Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();

        public void Initialize()
        {
            _hintButton.Initialize();
            _hintButton.OnHintRequested += HandleHintPress;
            
            DisableEnterButton();
            
            _enterButton.onClick.AddListener(HandleEnterPress);
            _backButton.onClick.AddListener(HandleBackPress);

            _keyGrid.Initialize(_keyboardLayout.Keys);
            _keyGrid.OnKeyPress += HandleKeyPress;
        }

        private void HandleHintPress()
        {
            OnHintPress.Invoke();
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
            _hintButton.Reset();
            DisableEnterButton();
        }

        public void EnableEnterButton()
        {
            _enterButton.interactable = true;
            _enterButtonColors.Enable();
        }

        public void DisableEnterButton()
        {
            _enterButton.interactable = false;
            _enterButtonColors.Disable();
        }

        public void MarkGuessed(char hintLetter)
        {
            _keyGrid.MarkGuessed(hintLetter);
        }
    }
}
