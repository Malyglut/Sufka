using System.Collections.Generic;
using System.Linq;

namespace Sufka.Validation
{
    public static class WordValidator
    {
        public static ValidationResult Validate(string word, char[] input)
        {
            var validationResult = new ValidationResult(input.Length);
            var letters = word.ToCharArray();
            Dictionary<char, int> letterCounts = new Dictionary<char, int>();

            foreach (var letter in letters)
            {
                if(!letterCounts.ContainsKey(letter))
                {
                    letterCounts.Add(letter, letters.Count(l => l == letter));
                }
            }
            
            for (int i = 0; i < input.Length; i++)
            {
                var letterCorrectness = LetterCorrectness.None;

                if (letters[i] == input[i])
                {
                    letterCorrectness = LetterCorrectness.Full;
                }
                else if (letters.Contains(input[i]))
                {
                    letterCorrectness = LetterCorrectness.Partial;
                }

                validationResult[i] = new LetterResult(input[i], letterCorrectness);
            }

            foreach (var letterResult in validationResult)
            {
                if (letterResult.result == LetterCorrectness.Full)
                {
                    letterCounts[letterResult.letter]--;
                }
            }
            
            foreach (var letterResult in validationResult)
            {
                if (letterResult.result == LetterCorrectness.Partial)
                {
                    if (letterCounts[letterResult.letter] > 0)
                    {
                        letterCounts[letterResult.letter]--;
                    }
                    else
                    {
                        letterResult.result = LetterCorrectness.None;
                    }
                }
            }
            
            validationResult.Check();
            
            return validationResult;
        }
    }
}
