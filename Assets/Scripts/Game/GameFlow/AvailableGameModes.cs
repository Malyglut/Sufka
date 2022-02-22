using System.Collections.Generic;
using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Unlocks
{
    [CreateAssetMenu(fileName = "Available Game Modes", menuName = "Sufka/Available Game Modes", order = 0)]
    public class AvailableGameModes : ScriptableObject
    {
        [SerializeField]
        private List<GameMode> _gameModes = new List<GameMode>();

        public List<GameMode> GameModes => new List<GameMode>(_gameModes);
        public int GameModesCount => _gameModes.Count;
    }
}
