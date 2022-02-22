using System;
using Sufka.Game.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Controls
{
    public class HintButton : MonoBehaviour
    {
        public event Action OnHintRequested;
        public event Action OnHintAdRequested;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _availableHints;

        [SerializeField]
        private ButtonColors _buttonColors;

        [SerializeField]
        private CurrentState _currentState;

        public void Refresh()
        {
            _availableHints.SetText($"x {_currentState.AvailableHints}");
        }

        private void TryGetHint()
        {
            if (_currentState.AvailableHints > 0)
            {
                OnHintRequested.Invoke();
                Disable();
            }
            else
            {
                OnHintAdRequested.Invoke();
            }

            Refresh();
        }

        private void Disable()
        {
            _button.interactable = false;
            _buttonColors.Disable();
        }

        public void Initialize()
        {
            _currentState.OnAvailableHintsUpdated += Refresh;
            _button.onClick.AddListener(TryGetHint);
            Refresh();
        }

        public void Reset(bool hintUsed)
        {
            if (hintUsed)
            {
                Disable();
            }
            else
            {
                _buttonColors.Enable();
                _button.interactable = true;
            }
        }
    }
}
