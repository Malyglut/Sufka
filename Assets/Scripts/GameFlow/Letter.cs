using TMPro;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class Letter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _letterText;

        public string CurrentLetter { get; private set; }
        
        public void SetLetter(string letter)
        {
            CurrentLetter = letter;
            _letterText.SetText(letter);
        }

        public void SetBlank()
        {
            SetLetter(string.Empty);
        }
    }
}
