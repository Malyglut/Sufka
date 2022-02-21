using System;
using Sufka.Game.GameFlow;
using Sufka.Game.Words;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class GameInProgressSaveData
    {
        public bool gameInProgress;
        public bool hintUsed;
        public Word targetWord;
        public WordLength wordLength;
    }
}
