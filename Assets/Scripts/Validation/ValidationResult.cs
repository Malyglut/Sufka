namespace Sufka.Validation
{
    public class ValidationResult
    {
        public LetterCorrectness[] Letters { get; }

        public bool FullMatch { get; private set; }

        public LetterCorrectness this[int i]
        {
            get => Letters[i];
            set => Letters[i] = value;
        }

        public ValidationResult(int capacity)
        {
            Letters = new LetterCorrectness[capacity];
        }

        public override string ToString()
        {
            var text = "[ ";

            for (var i = 0; i < Letters.Length; i++)
            {
                text += Letters[i];
                text += i < Letters.Length - 1 ? ", " : " ]";
            }

            return text;
        }

        public void Check()
        {
            var full = 0;

            foreach (var letter in Letters)
            {
                if (letter == LetterCorrectness.Full)
                {
                    full++;
                }
            }

            FullMatch = full == Letters.Length;
        }
    }
}
