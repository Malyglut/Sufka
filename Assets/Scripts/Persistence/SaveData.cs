using System;
using Sufka.Statistics;

namespace Sufka.Persistence
{
    [Serializable]
    public class SaveData
    {
        public int score;
        public int availableHints;
        public WordStatistics[] wordStatistics;
    }
}
