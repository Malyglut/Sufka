using System;
using Sufka.Validation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.GameFlow
{
    public class Letter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _letterText;

        [SerializeField]
        private Image _background;
        
        [SerializeField]
        private Color _startingColor = Color.white;
        
        [SerializeField]
        private Color _fullCorrectColor = Color.white;
        
        [SerializeField]
        private Color _partialCorrectColor = Color.white;
        
        [SerializeField]
        private Color _noneCorrectColor = Color.white;

        public char CurrentLetter { get; private set; }
        public LetterCorrectness CurrentCorrectness { get; private set; } = LetterCorrectness.NotSet; 
        
        public void SetLetter(char letter)
        {
            CurrentLetter = letter;
            _letterText.SetText(letter.ToString());
        }

        public void SetBlank()
        {
            SetLetter('\0');
        }

        public void Display(LetterCorrectness letterCorrectness)
        {
            CurrentCorrectness = letterCorrectness;
            var color = _noneCorrectColor;
            
            switch (letterCorrectness)
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
                case LetterCorrectness.NotSet:
                    color = _startingColor;
                    break;
                default:
                    color = _startingColor;
                    break;
            }

            _background.color = color;
        }

        public void Reset(bool resetLetter)
        {
            if(resetLetter)
            {
                SetBlank();
            }

            CurrentCorrectness = LetterCorrectness.NotSet;
            _background.color = _startingColor;
        }
    }
}
