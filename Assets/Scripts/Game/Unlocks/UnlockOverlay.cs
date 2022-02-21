using System;
using Sufka.Game.Colors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sufka.Game.Unlocks
{
    public class UnlockOverlay : MonoBehaviour, IPointerDownHandler
    {
        public event Action OnClick;
        
        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;
        
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
            OnClick.Invoke();
        }

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }
    }
}
