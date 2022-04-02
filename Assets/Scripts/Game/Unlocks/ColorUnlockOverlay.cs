using System;
using Sufka.Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Unlocks
{
    public class ColorUnlockOverlay : MonoBehaviour
    {
        public event Action OnUnlockRequested;
        
        [SerializeField]
        private UnlockOverlay _unlockOverlay;

        [SerializeField]
        private ClickableScrollElement _clickableScrollElement;

        public void Initialize(ScrollRect scrollRect)
        {
            _unlockOverlay.Initialize();
            _unlockOverlay.OnClick += RequestColorUnlock;
            
            _clickableScrollElement.Initialize(scrollRect);
            _clickableScrollElement.OnDragStarted += InterruptClick;
        }

        private void InterruptClick()
        {
            _unlockOverlay.InterruptClick();
        }

        private void RequestColorUnlock()
        {
            OnUnlockRequested.Invoke();
        }

        public void Disable()
        {
            _unlockOverlay.Disable();
        }

        public void Enable()
        {
            _unlockOverlay.Enable();
        }
    }
}
