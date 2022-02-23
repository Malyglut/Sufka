using System;
using System.Collections.Generic;
using Sufka.Game.Statistics;
using UnityEngine;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class SaveData
    {
        private const int INITIAL_AVAILABLE_HINTS = 10;
        private const int INITIAL_WORDS_UNTIL_HINT_REWARD = 5;
        private const int INITIAL_WORDS_UNTIL_BONUS_POINTS_REWARD = 10;

        public int score;
        public int availableHints = INITIAL_AVAILABLE_HINTS;
        public WordStatistics[] wordStatistics;

        public int selectedColorSchemeIdx;

        public int unlockedColorCount;
        public int pointsSpentOnColors;
        public int pointsSpentOnUnlocks;

        public int wordsUntilHintReward = INITIAL_WORDS_UNTIL_HINT_REWARD;
        public int wordsUntilBonusPointsReward = INITIAL_WORDS_UNTIL_BONUS_POINTS_REWARD;
        public int bonusPointsReward;

        public bool tutorialCompleted;

        public bool ratingProposed;
        public int lastRatingProposedWordCount;

        public List<bool> unlockedColors = new List<bool> {true};
        public List<bool> unlockedGameModes = new List<bool> {true};

        public void UpdateColorUnlocksCount(int colorSchemeCount)
        {
            var difference = Mathf.Abs(unlockedColors.Count - colorSchemeCount);

            for (var i = 0; i < difference; i++)
            {
                unlockedColors.Add(false);
            }
        }

        public void UpdateGameModeUnlocksCount(int gameModesCount)
        {
            var difference = Mathf.Abs(unlockedGameModes.Count - gameModesCount);

            for (var i = 0; i < difference; i++)
            {
                unlockedGameModes.Add(false);
            }
        }

        public void ResetWordsUntilHintReward()
        {
            wordsUntilHintReward = INITIAL_WORDS_UNTIL_HINT_REWARD;
        }

        public void ResetWordsUntilBonusPointsReward()
        {
            wordsUntilBonusPointsReward = INITIAL_WORDS_UNTIL_BONUS_POINTS_REWARD;
            bonusPointsReward = 0;
        }
    }
}
