using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Unlocks
{
    [Serializable]
    public class UnlocksData
    {
        public int selectedColorSchemeIdx;
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
