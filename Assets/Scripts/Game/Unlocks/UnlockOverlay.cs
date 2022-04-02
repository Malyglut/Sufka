using System;
using Sufka.Game.Colors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sufka.Game.Unlocks
{
    public class UnlockOverlay : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnClick;
        
        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;
        
        private bool _pointerOver;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //do nothing
        }

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_pointerOver)
            {
                OnClick.Invoke();
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

        public void InterruptClick()
        {
            _pointerOver = false;
        }
    }
}
