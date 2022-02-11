using System;
using Sufka.Game.Statistics;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class SaveData
    {
        public int score;
        public int availableHints;
        public WordStatistics[] wordStatistics;
    }
}
