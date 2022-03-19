using System;
using Sufka.Game.Colors;
using Sufka.Game.Validation;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class InteractiveLetter : MonoBehaviour
    {
        [SerializeField]
        private LetterDisplay _letterDisplay;

        [SerializeField]
        private LetterCorrectnessFill _fill;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        public LetterCorrectness CurrentCorrectness { get; private set; } = LetterCorrectness.NotSet;
        public char CurrentLetter => _letterDisplay.CurrentLetter;
        public bool IsBlank => _letterDisplay.IsBlank;
        public LetterResult CurrentLetterResult => new LetterResult(CurrentLetter, CurrentCorrectness);

        public void Display(LetterCorrectness letterCorrectness)
        {
            CurrentCorrectness = letterCorrectness;

            var active = CurrentCorrectness != LetterCorrectness.NotSet;

            if (active)
            {
                var currentColorScheme = ColorSchemeController.CurrentColorScheme;
                var color = Color.white;

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
                }
                
                _fill.Refresh(color);
            }
            else
            {
                _fill.Disable();
            }
        }

        public void SetEmpty()
        {
            CurrentCorrectness = LetterCorrectness.NotSet;
            _letterDisplay.SetBlank();
            _fill.Disable();
        }

        public void SetLetter(char letter)
        {
            _letterDisplay.SetLetter(letter);

            if (_letterDisplay.IsBlank)
            {
                SetEmpty();
            }
        }

        public void MarkGuessed(char letter)
        {
            CurrentCorrectness = LetterCorrectness.Full;
            _letterDisplay.SetLetter(letter);
            _fill.Refresh(ColorSchemeController.CurrentColorScheme.GetColor(ColorWeight.FullCorrect));
        }

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }
    }
}
