using System;
using Sufka.GameFlow;
using Sufka.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Controls
{
    public class LetterKey : MonoBehaviour
    {
        public event Action<char> OnPress;
        
        [SerializeField]
        private Button _button;
        
        [SerializeField]
        private Letter _letter;

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
            if(letterCorrectness > _letter.CurrentCorrectness)
            {
                _letter.Display(letterCorrectness);
                _button.interactable = letterCorrectness != LetterCorrectness.None;
            }
        }

        public void Reset()
        {
            _button.interactable = true;
            _letter.Reset(false);
        }
    }
}
