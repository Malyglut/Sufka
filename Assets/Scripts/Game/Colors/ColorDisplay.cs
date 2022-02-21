using System;
using Sufka.Game.Unlocks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sufka.Game.Colors
{
    public class ColorDisplay : MonoBehaviour, IPointerDownHandler
    {
        public event Action<ColorScheme> OnColorPicked;
        public event Action<ColorScheme> OnUnlockRequested;
        
        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _primary;
        
        [SerializeField]
        private Image _secondary;

        [SerializeField]
        private ColorUnlockOverlay _unlockOverlay;
        
        private ColorScheme _colorScheme;

        public void Initialize(ColorScheme colorScheme, bool unlocked)
        {
            _colorScheme = colorScheme;
            _name.SetText(colorScheme.Name);
            _primary.color = colorScheme.GetColor(ColorWeight.Primary);
            _secondary.color = colorScheme.GetColor(ColorWeight.Secondary);

            _unlockOverlay.OnUnlockRequested += RequestColorSchemeUnlock;
            _unlockOverlay.Initialize();

            RefreshAvailability(unlocked);
        }

        private void RequestColorSchemeUnlock()
        {
            OnUnlockRequested.Invoke(_colorScheme);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnColorPicked.Invoke(_colorScheme);
        }

        public void RefreshAvailability(bool unlocked)
        {
            if (unlocked)
            {
                _unlockOverlay.Disable();
            }
            else
            {
                _unlockOverlay.Enable();
            }
        }
    }
}
