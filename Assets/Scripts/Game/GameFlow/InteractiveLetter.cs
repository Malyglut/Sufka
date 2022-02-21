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

        public void Display(LetterCorrectness letterCorrectness)
        {
            CurrentCorrectness = letterCorrectness;

            var active = CurrentCorrectness == LetterCorrectness.Full || CurrentCorrectness == LetterCorrectness.Partial;

            if(active)
            {
                var color = CurrentCorrectness == LetterCorrectness.Full ? _fullCorrectColor : _partialCorrectColor;
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
        }

        public void MarkGuessed(char letter)
        {
            _letterDisplay.SetLetter(letter);
            _fill.Refresh(_fullCorrectColor);
        }

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }
    }
}
