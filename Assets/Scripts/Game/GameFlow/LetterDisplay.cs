using TMPro;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class LetterDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _letterText;

        public char CurrentLetter { get; private set; }
        public bool IsBlank => CurrentLetter == '\0';

        public void SetLetter(char letter)
        {
            CurrentLetter = letter;
            _letterText.SetText(letter.ToString());
        }

        public void SetBlank()
        {
            SetLetter('\0');
        }
    }
}
