using System.Collections;
using System.Collections.Generic;

namespace Sufka.Game.Validation
{
    public class ValidationResult : IEnumerable<LetterResult>
    {
        public LetterResult[] Letters { get; }

        public bool FullMatch { get; private set; }

        public List<int> GuessedIndices { get; } = new List<int>();

        public LetterResult this[int i]
        {
            get => Letters[i];
            set => Letters[i] = value;
        }

        public ValidationResult(int capacity)
        {
            Letters = new LetterResult[capacity];
        }

        public IEnumerator<LetterResult> GetEnumerator()
        {
            return ((IEnumerable<LetterResult>)Letters).GetEnumerator();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Check()
        {
            var full = 0;

            for (int i = 0; i < Letters.Length; i++)
            {
                if (Letters[i].result == LetterCorrectness.Full)
                {
                    full++;
                    GuessedIndices.Add(i);
                }
            }

            FullMatch = full == Letters.Length;
        }
    }
}
