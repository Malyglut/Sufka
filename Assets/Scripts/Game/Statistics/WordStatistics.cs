using System;
using Sufka.GameFlow;

namespace Sufka.Statistics
{
    [Serializable]
    public class WordStatistics
    {
        public WordLength wordLength;
        public int guessedWords;
        public int hintsUsed;
        public int firstAttemptGuesses;
        public int secondAttemptGuesses;

        public WordStatistics(WordLength wordLength)
        {
            this.wordLength = wordLength;
        }
    }
}
