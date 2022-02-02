using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Utility
{
    [ExecuteInEditMode]
    public class SafeAreaScaler : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private RectTransform _safeArea;

        private Rect _currentSafeArea = Rect.zero;
        private ScreenOrientation _currentOrientation;

        void Start()
        {
            CacheScreenSettings();

            ScaleSafeArea();
        }

        private void CacheScreenSettings()
        {
            _currentOrientation = Screen.orientation;
            _currentSafeArea = Screen.safeArea;
        }

        private void ScaleSafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            Rect pixelRect = _canvas.pixelRect;
            
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;

            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            _safeArea.anchorMin = anchorMin;
            _safeArea.anchorMax = anchorMax;
            
            CacheScreenSettings();
        }

        void Update()
        {
            if (Screen.orientation != _currentOrientation || _currentSafeArea != Screen.safeArea)
            {
                ScaleSafeArea();
            }
        }
    }
}