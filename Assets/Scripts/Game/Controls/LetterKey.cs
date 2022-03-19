using System;
using Sufka.Game.Colors;
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
        private ColorSchemeInitializer _colorSchemeInitializer;

        public LetterCorrectness CurrentCorrectness { get; private set; } = LetterCorrectness.NotSet;

        public void Initialize(char character)
        {
            _button.onClick.AddListener(Press);
            _letter.SetLetter(character);

            _colorSchemeInitializer.Initialize();
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

                var currentColorScheme = ColorSchemeController.CurrentColorScheme;

                switch (CurrentCorrectness)
                {
                    case LetterCorrectness.None:
                        color = currentColorScheme.GetColor(ColorWeight.Disabled);
                        break;
                    case LetterCorrectness.Partial:
                        color = currentColorScheme.GetColor(ColorWeight.PartialCorrect);
                        break;
                    case LetterCorrectness.Full:
                        color = currentColorScheme.GetColor(ColorWeight.FullCorrect);
                        break;
                    default:
                        color = currentColorScheme.GetColor(ColorWeight.Disabled);
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
