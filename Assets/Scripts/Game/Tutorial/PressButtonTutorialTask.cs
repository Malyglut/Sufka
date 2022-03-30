using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Tutorial
{
    public class PressButtonTutorialTask : MonoBehaviour
    {
        [SerializeField]
        private TutorialStep _step;

        [SerializeField]
        private Button _button;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _button.onClick.AddListener(Complete);
        }

        private void Complete()
        {
            _button.onClick.RemoveListener(Complete);
            _step.Complete();
        }
    }
}
