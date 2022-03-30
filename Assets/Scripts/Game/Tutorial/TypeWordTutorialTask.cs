using Sufka.Game.GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Tutorial
{
    public class TypeWordTutorialTask : MonoBehaviour
    {
        [SerializeField]
        private TutorialStep _step;

        [SerializeField]
        private Button _hideButton;

        [SerializeField]
        private PlayArea _playArea;
        
        [SerializeField]
        private PlayAreaController _playAreaController;

        [SerializeField]
        private string _targetWord;

        [SerializeField]
        private bool _showIntermediateStep;

        [SerializeField]
        private TutorialStep _intermediateStep;

        private void Start()
        {
            Initialize();
        }

        private void CheckWord()
        {
            var typedWord = new string(_playArea.CurrentWord);

            if (typedWord == _targetWord.ToUpper())
            {
                Complete();
            }
            else
            {
                _playAreaController.TutorialDisableEnter();

                if (_showIntermediateStep && !_intermediateStep.Shown)
                {
                    _intermediateStep.Show(null);
                }
            }
        }

        private void Initialize()
        {
            _playArea.OnCurrentRowFull += CheckWord;
            _hideButton.onClick.AddListener(_step.Hide);
        }

        private void Complete()
        {
            _playArea.OnCurrentRowFull -= CheckWord;
            _step.Complete();
        }
    }
}
