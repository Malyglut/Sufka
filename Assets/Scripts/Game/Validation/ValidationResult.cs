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
            CorrectLetterCount = 0;
            CorrectSpotCount = 0;

            for (int i = 0; i < Letters.Length; i++)
            {
                var letterCorrectness = Letters[i].result;

                if (letterCorrectness == LetterCorrectness.Full || letterCorrectness == LetterCorrectness.Partial)
                {
                    CorrectLetterCount++;
                }
                
                if (letterCorrectness == LetterCorrectness.Full)
                {
                    
                    CorrectSpotCount++;
                    GuessedIndices.Add(i);
                }
            }

            FullMatch = CorrectSpotCount == Letters.Length;
        }

        public int CorrectLetterCount { get; private set; }

        public int CorrectSpotCount { get; private set; }
    }
}
