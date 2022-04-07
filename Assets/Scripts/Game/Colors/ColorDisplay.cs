using System;
using Sufka.Game.Unlocks;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sufka.Game.Colors
{
    public class ColorDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ColorScheme> OnColorPicked = EventUtility.Empty;
        public event Action<ColorScheme> OnUnlockRequested = EventUtility.Empty;
        
        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _primary;
        
        [SerializeField]
        private Image _secondary;

        [SerializeField]
        private ColorUnlockOverlay _unlockOverlay;

        [SerializeField]
        private ClickableScrollElement _clickableScrollElement;
        
        private ColorScheme _colorScheme;
        private bool _pointerOver;

        public void Initialize(ColorScheme colorScheme, bool unlocked, ScrollRect scrollRect)
        {
            _colorScheme = colorScheme;
            _name.SetText(colorScheme.Name);
            _primary.color = colorScheme.GetColor(ColorWeight.Primary);
            _secondary.color = colorScheme.GetColor(ColorWeight.Secondary);

            _unlockOverlay.OnUnlockRequested += RequestColorSchemeUnlock;
            _unlockOverlay.Initialize(scrollRect);

            _clickableScrollElement.Initialize(scrollRect);
            _clickableScrollElement.OnDragStarted+= InterruptClick;

            RefreshAvailability(unlocked);
        }

        private void InterruptClick()
        {
            _pointerOver = false;
        }

        private void RequestColorSchemeUnlock()
        {
            OnUnlockRequested.Invoke(_colorScheme);
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

        public void OnPointerDown(PointerEventData eventData)
        {
            //do nothing
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_pointerOver)
            {
                OnColorPicked.Invoke(_colorScheme);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
        }
    }
}
