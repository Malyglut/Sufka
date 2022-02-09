using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.GameFlow
{
    [CreateAssetMenu(fileName = "Game Setup Database", menuName = "Sufka/Game Setup Database")]
    public class GameSetupDatabase : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<WordLength, GameSetup> _gameSetups = new Dictionary<WordLength, GameSetup>();
        public GameSetup this[WordLength wordLength] => _gameSetups[wordLength];
    }
}
