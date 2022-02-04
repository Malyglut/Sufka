using System;
using System.Collections.Generic;
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

        [SerializeField]
        private KeyGrid _keyGrid;
        
        [SerializeField]
        private Button _enterButton;
        
        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private KeyboardLayout _keyboardLayout;
        

        private Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();

        public void Initialize()
        {
            _enterButton.onClick.AddListener(HandleEnterPress);
            _backButton.onClick.AddListener(HandleBackPress);
            _keyGrid.OnKeyPress += HandleKeyPress;

            _keyGrid.Initialize(_keyboardLayout.Keys);
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
        }
    }
}
