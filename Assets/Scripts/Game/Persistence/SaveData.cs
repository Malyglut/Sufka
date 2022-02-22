using System;
using Sufka.Game.Statistics;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class SaveData
    {
        private const int INITIAL_AVAILABLE_HINTS = 10;
        
        public int score;
        public int availableHints = INITIAL_AVAILABLE_HINTS;
        public WordStatistics[] wordStatistics;
    }
}
