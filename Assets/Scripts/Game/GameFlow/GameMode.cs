using UnityEngine;

namespace Sufka.Game.GameFlow
{
    [CreateAssetMenu(fileName = "Game Mode", menuName = "Sufka/Game Mode", order = 0)]
    public class GameMode : ScriptableObject
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private int _unlockCost;
        public string Name => _name;
        public int UnlockCost => _unlockCost;
    }
}
