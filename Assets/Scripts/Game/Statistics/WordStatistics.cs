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

        public WordStatistics(int gameModeIdx)
        {
            this.gameModeIdx = gameModeIdx;
        }
    }
}
