using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Game.Utility
{
    public class ScreenshotTaker : MonoBehaviour
    {
        [SerializeField]
        private int _superSize;

#if UNITY_EDITOR
        [Button]
        private void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot($"{Application.dataPath}/Screenshots/screen{DateTime.Now}.jpg",
                                            _superSize);
        }
#endif
    }
}
