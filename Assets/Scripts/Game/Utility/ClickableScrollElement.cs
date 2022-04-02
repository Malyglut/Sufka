using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sufka.Game.Utility
{
    public class ClickableScrollElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action OnDragStarted = EventUtility.Empty;
        
        private ScrollRect _scrollRect;

        public void Initialize(ScrollRect scrollRect)
        {
            _scrollRect = scrollRect;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnDrag(eventData);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragStarted.Invoke();
            
            if (_scrollRect != null)
            {
                _scrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnEndDrag(eventData);
            }
        }
    }
}
