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
        private Image _getHintsIcon;

        [SerializeField]
        private ButtonColors _buttonColors;

        [SerializeField]
        private GameController _gameController;

        private int AvailableHints =>_gameController != null ? _gameController.AvailableHints : 1;
        
        public void Refresh()
        {
            _availableHints.SetText($"x {AvailableHints}");
            
            _availableHints.gameObject.SetActive(AvailableHints > 0);
            _getHintsIcon.gameObject.SetActive(AvailableHints == 0);

            if (_gameController!= null && _gameController.HintUsed)
            {
                Disable();
            }

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
            if(AvailableHints>0)
            {
                _button.interactable = false;
                _buttonColors.Disable();
            }
            else
            {
                _availableHints.gameObject.SetActive(false);
                _getHintsIcon.gameObject.SetActive(true);
            }
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
                
                _availableHints.gameObject.SetActive(AvailableHints > 0);
                _getHintsIcon.gameObject.SetActive(AvailableHints == 0);
            }
        }
    }
}
