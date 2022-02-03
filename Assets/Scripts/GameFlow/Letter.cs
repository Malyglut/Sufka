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
        private Color _fullCorrectColor = Color.white;
        
        [SerializeField]
        private Color _partialCorrectColor = Color.white;
        
        [SerializeField]
        private Color _noneCorrectColor = Color.white;

        public char CurrentLetter { get; private set; }
        
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
                default:
                    color = _noneCorrectColor;
                    break;
            }

            _background.color = color;
        }
    }
}
