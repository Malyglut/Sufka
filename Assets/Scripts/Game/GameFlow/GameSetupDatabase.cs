using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    [CreateAssetMenu(fileName = "Game Setup Database", menuName = "Sufka/Game Setup Database")]
    public class GameSetupDatabase : ScriptableObject
    {
        [SerializeField]
        private List<WordLengthGameSetup> _setups;
        public GameSetup this[WordLength wordLength] =>
            _setups.First(setup => setup.wordLength == wordLength).gameSetup;
    }
}
