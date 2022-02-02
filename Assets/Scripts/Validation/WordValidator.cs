using System;

namespace Sufka.Validation
{
    public static class WordValidator
    {
        public static ValidationResult Validate(string word, string[] input)
        {
            var validationResult = new ValidationResult(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                var letter = word.Substring(i, 1);

                var letterCorrectness = string.Equals(letter, input[i], StringComparison.InvariantCultureIgnoreCase) ?
                                            LetterCorrectness.Full :
                                            LetterCorrectness.None;



                validationResult[i] = letterCorrectness;
            }

            validationResult.Check();
            
            return validationResult;
        }
    }
}
