using System;

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
        public int typedLetters;
        public int removedLetters;
        public int lettersInCorrectSpot;
        public int correctLetters;

        public WordStatistics(int gameModeIdx)
        {
            this.gameModeIdx = gameModeIdx;
        }
    }
}
