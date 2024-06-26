using System;
using System.Collections.Generic;
using Sufka.Game.DailyTasks;
using Sufka.Game.Statistics;
using Sufka.Game.Utility;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class SaveData
    {
        private const int INITIAL_AVAILABLE_HINTS = 10;
        private const int INITIAL_WORDS_UNTIL_HINT_REWARD = 5;
        private const int INITIAL_WORDS_UNTIL_BONUS_POINTS_REWARD = 10;
        private const string DEFAULT_COLOR_ID = "704d92bd-f3cd-44aa-b6d7-dacde611e13e";
        private const string DEFAULT_GAME_MODE_ID = "fb9cb282-07c1-41cb-bb88-fd1dc859d738";

        public int score;
        public int availableHints = INITIAL_AVAILABLE_HINTS;
        public WordStatistics[] wordStatistics;

        public int selectedColorSchemeIdx;
        public string selectedColorSchemeId = DEFAULT_COLOR_ID;

        public int pointsSpentOnColors;
        public int pointsSpentOnUnlocks;

        public int wordsUntilHintReward = INITIAL_WORDS_UNTIL_HINT_REWARD;
        public int wordsUntilBonusPointsReward = INITIAL_WORDS_UNTIL_BONUS_POINTS_REWARD;
        public int bonusPointsReward;

        public bool tutorialCompleted;
        public int tutorialCompletionsCount;

        public List<bool> unlockedColors;
        public List<bool> unlockedGameModes;

        public List<string> unlockedColorIds = new List<string> {DEFAULT_COLOR_ID};
        public List<string> unlockedGameModeIds = new List<string> {DEFAULT_GAME_MODE_ID};
        public List<string> completedAchievements = new List<string>();

        public List<DailyTaskData> dailyTasksData = new List<DailyTaskData>();
        public List<string> previousDailyTasks = new List<string>();
        public DateTimeSaveData nextDailyTasksGenerationDateTime;
        public int completedDailyTasksCount;
        public int pointsGainedFromTasks;

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
