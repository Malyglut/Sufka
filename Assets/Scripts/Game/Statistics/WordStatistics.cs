using System;
using Sufka.Game.GameFlow;

namespace Sufka.Game.Statistics
{
    [Serializable]
    public class WordStatistics
    {
        public int gameModeIdx;
        public int guessedWords;
        public int hintsUsed;
        public int firstAttemptGuesses;
        public int secondAttemptGuesses;
        public int thirdAttemptGuesses;
        public int fourthAttemptGuesses;
        public int fifthAttemptGuesses;
        public int scoreGained;

        public WordStatistics(int gameModeIdx)
        {
            this.gameModeIdx = gameModeIdx;
        }
    }
}
