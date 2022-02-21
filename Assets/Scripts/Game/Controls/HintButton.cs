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
        private GameController _gameController;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _availableHints;

        [SerializeField]
        private ButtonColors _buttonColors;

        private void Refresh()
        {
            _availableHints.SetText($"x {_gameController.AvailableHints}");
        }

        private void TryGetHint()
        {
            if (_gameController.AvailableHints > 0)
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
            _gameController.OnAvailableHintsUpdated += Refresh;
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
