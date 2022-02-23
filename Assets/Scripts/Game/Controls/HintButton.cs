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
        private GameController _gameController;

        private int AvailableHints =>_gameController != null ? _gameController.AvailableHints : 1;
        
        public void Refresh()
        {
            _availableHints.SetText($"x {AvailableHints}");
        }

        private void TryGetHint()
        {
            if (AvailableHints > 0)
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
