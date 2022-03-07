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
        private Color _fullCorrectColor = Color.white;

        [SerializeField]
        private Color _partialCorrectColor = Color.white;

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
                var color = CurrentCorrectness == LetterCorrectness.Full ? _fullCorrectColor : _partialCorrectColor;

                switch (CurrentCorrectness)
                {
                    case LetterCorrectness.None:
                        color = ColorSchemeController.CurrentColorScheme.GetColor(ColorWeight.Disabled);
                        break;
                    case LetterCorrectness.Partial:
                        color = _partialCorrectColor;
                        break;
                    case LetterCorrectness.Full:
                        color = _fullCorrectColor;
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
            _fill.Refresh(_fullCorrectColor);
        }

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }
    }
}
