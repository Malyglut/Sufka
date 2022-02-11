using System;
using Sufka.Game.Words;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    [Serializable]
    public class GameSetup
    {
        [SerializeField]
        private int _letterCount;
        
        [SerializeField]
        private int _attemptCount;

        [SerializeField]
        private WordAtlas _wordAtlas;

        public int LetterCount => _letterCount;
        public int AttemptCount => _attemptCount;
        public WordAtlas WordAtlas => _wordAtlas;
    }
}
