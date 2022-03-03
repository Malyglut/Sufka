using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Game.Utility
{
    public class ScreenshotTaker : MonoBehaviour
    {
        [SerializeField]
        private int _superSize;

        private int _screenshotNumber;
#if UNITY_EDITOR
        [Button]
        private void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot($"{Application.dataPath}/Screenshots/screen{_screenshotNumber++}.jpg",
                                            _superSize);
        }
#endif
    }
}
