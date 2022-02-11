using System;
using Sufka.Ads;
using Sufka.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Controls
{
    public class HintButton : MonoBehaviour
    {
        public event Action OnHintRequested;
        
        [SerializeField]
        private PlayAreaController _playArea;
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _availableHints;



        [SerializeField]
        private ButtonColors _buttonColors;
        
        private void Refresh()
        {
            _availableHints.SetText($"x {_playArea.AvailableHints}");
        }

        private void TryGetHint()
        {
            if (_playArea.AvailableHints > 0)
            {
                OnHintRequested.Invoke();
                _button.interactable = false;
                _buttonColors.Disable();
            }
            else
            {
               AdsController.ShowHintAd();
            }
            
            Refresh();
        }

        public void Initialize()
        {
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
