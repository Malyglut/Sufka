using System;
using System.Collections.Generic;
using Sufka.Game.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Controls
{
    public class Keyboard : MonoBehaviour
    {
        public event Action<char> OnKeyPress;
        public event Action OnEnterPress;
        public event Action OnBackPress;
        public event Action OnHintPress;
        public event Action OnHintAdPress;

        [SerializeField]
        private KeyGrid _keyGrid;

        [SerializeField]
        private Button _enterButton;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private HintButton _hintButton;

        [SerializeField]
        private KeyboardLayoutData _keyboardLayout;

        [SerializeField]
        private ButtonColors _enterButtonColors;

        private Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();

        public void Initialize()
        {
            _hintButton.Initialize();
            _hintButton.OnHintRequested += HandleHintPress;
            _hintButton.OnHintAdRequested += HandleHintAdPress;

            DisableEnterButton();

            _enterButton.onClick.AddListener(HandleEnterPress);
            _backButton.onClick.AddListener(HandleBackPress);

            _keyGrid.Initialize(_keyboardLayout.Keys);
            _keyGrid.OnKeyPress += HandleKeyPress;
        }

        private void HandleHintAdPress()
        {
            OnHintAdPress.Invoke();
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
            DisableEnterButton();
        }

        public void Restore(bool hintUsed)
        {
            _keyGrid.Reset();
            _hintButton.Reset(hintUsed);
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

        public void RestoreKeys(List<List<LetterResult>> filledLetters)
        {
            _keyGrid.Restore(filledLetters);
        }

        public void RefreshHints()
        {
            _hintButton.Refresh();
        }
    }
}
