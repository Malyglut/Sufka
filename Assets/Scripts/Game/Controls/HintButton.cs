using System;
using Sufka.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Controls
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
                _button.interactable = false;
                _buttonColors.Disable();
            }
            else
            {
                OnHintAdRequested.Invoke();
            }

            Refresh();
        }

        public void Initialize()
        {
            _gameController.OnAvailableHintsUpdated += Refresh;
            _button.onClick.AddListener(TryGetHint);
            Refresh();
        }

        public void Reset()
        {
            _buttonColors.Enable();
            _button.interactable = true;
        }
    }
}
