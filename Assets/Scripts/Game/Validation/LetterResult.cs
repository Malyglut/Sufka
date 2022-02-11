namespace Sufka.Game.Validation
{
    public class LetterResult
    {
        public readonly char letter;
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
