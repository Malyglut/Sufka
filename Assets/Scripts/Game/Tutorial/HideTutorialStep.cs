using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Tutorial
{
    public class HideTutorialStep : MonoBehaviour
    {
        [SerializeField]
        private TutorialStep _step;

        [SerializeField]
        private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(HideStep);
        }

        private void HideStep()
        {
            _button.onClick.RemoveListener(HideStep);
            _step.Hide();
        }
    }
}
