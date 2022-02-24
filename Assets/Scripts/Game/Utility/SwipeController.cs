using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sufka.Game.Utility
{
    public class SwipeController : MonoBehaviour, IEndDragHandler, IDragHandler
    {
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;
        
        public void OnEndDrag(PointerEventData eventData)
        {
                Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
                var direction = GetDragDirection(dragVectorDirection);

                switch (direction)
                {
                    case DraggedDirection.Right:
                        OnSwipeRight.Invoke();
                        break;
                    case DraggedDirection.Left:
                        OnSwipeLeft.Invoke();
                        break;
                }
        }
        
        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x);
            var positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            
            if (positiveX > positiveY)
            {
                draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;
            }
            
            return draggedDir;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //
        }
    }
}
