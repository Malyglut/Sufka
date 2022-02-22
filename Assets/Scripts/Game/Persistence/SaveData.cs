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
        
        public int score;
        public int availableHints = INITIAL_AVAILABLE_HINTS;
        public WordStatistics[] wordStatistics;
        
        public int selectedColorSchemeIdx;

        public int unlockedColorCount;
        public int pointsSpentOnColors;
        public int pointsSpentOnUnlocks;
        
        public List<bool> unlockedColors = new List<bool> {true};
        public List<bool> unlockedGameModes = new List<bool> {true};
        
        public void UpdateColorUnlocksCount(int colorSchemeCount)
        {
            var difference = Mathf.Abs(unlockedColors.Count - colorSchemeCount);

            for (int i = 0; i < difference; i++)
            {
                unlockedColors.Add(false);
            }
        }

        public void UpdateGameModeUnlocksCount(int gameModesCount)
        {
            var difference = Mathf.Abs(unlockedGameModes.Count - gameModesCount);

            for (int i = 0; i < difference; i++)
            {
                unlockedGameModes.Add(false);
            }
        }
    }
}
