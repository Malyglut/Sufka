using System;
using UnityEngine;

namespace Sufka.Game.Unlocks
{
    public class ColorUnlockOverlay : MonoBehaviour
    {
        public event Action OnUnlockRequested;
        
        [SerializeField]
        private UnlockOverlay _unlockOverlay;

        public void Initialize()
        {
            _unlockOverlay.Initialize();
            _unlockOverlay.OnClick += RequestColorUnlock;
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
