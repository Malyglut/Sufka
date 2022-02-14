using System;
using Sufka.Game.GameFlow;
using Sufka.Game.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Controls
{
    public class LetterKey : MonoBehaviour
    {
        public event Action<char> OnPress;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private LetterDisplay _letter;

        [SerializeField]
        private LetterCorrectnessFill _fill;

        [SerializeField]
        private Color _fullCorrectColor = Color.white;

        [SerializeField]
        private Color _partialCorrectColor = Color.white;

        [SerializeField]
        private Color _noneCorrectColor = Color.white;

        public LetterCorrectness CurrentCorrectness { get; private set; } = LetterCorrectness.NotSet;

        public void Initialize(char character)
        {
            _button.onClick.AddListener(Press);
            _letter.SetLetter(character);
        }

        private void Press()
        {
            OnPress.Invoke(_letter.CurrentLetter);
        }

        public void Display(LetterCorrectness letterCorrectness)
        {
            if (letterCorrectness > CurrentCorrectness)
            {
                CurrentCorrectness = letterCorrectness;

                var letterAvailable = letterCorrectness != LetterCorrectness.None;

                Color color;

                switch (CurrentCorrectness)
                {
                    case LetterCorrectness.None:
                        color = _noneCorrectColor;
                        break;
                    case LetterCorrectness.Partial:
                        color = _partialCorrectColor;
                        break;
                    case LetterCorrectness.Full:
                        color = _fullCorrectColor;
                        break;
                    default:
                        color = _noneCorrectColor;
                        break;
                }

                _fill.Refresh(color);

                _button.interactable = letterAvailable;
            }
        }

        public void Reset()
        {
            CurrentCorrectness = LetterCorrectness.NotSet;
            _button.interactable = true;
            _fill.Disable();
        }
    }
}
