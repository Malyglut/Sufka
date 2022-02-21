using Sufka.Game.Colors;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class NonInteractiveLetter : MonoBehaviour
    {
        [SerializeField]
        private LetterDisplay _letter;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        public void Initialize()
        {
            _colorSchemeInitializer.Initialize();
        }

        public void SetLetter(char letter)
        {
            _letter.SetLetter(letter);
        }
    }
}
