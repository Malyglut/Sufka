using System;

namespace Sufka.Game.Validation
{
    [Serializable]
    public class LetterResult
    {
        public char letter;
        public LetterCorrectness result;

        public LetterResult(char letter, LetterCorrectness result)
        {
            this.letter = letter;
            this.result = result;
        }

        public override string ToString()
        {
            return $"{letter} - {result}";
        }
    }
}
