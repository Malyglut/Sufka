using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    [CreateAssetMenu(fileName = "Game Mode", menuName = "Sufka/Game Mode", order = 0)]
    public class GameMode : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private string _gameModeId = Guid.Empty.ToString();

        [SerializeField]
        private string _name;

        [SerializeField]
        private int _unlockCost;

        [SerializeField]
        private int _orderInList;
        
        public string Name => _name;
        public int UnlockCost => _unlockCost;
        public string GameModeId => _gameModeId;
        public int OrderInList => _orderInList;

#if UNITY_EDITOR
        [Button, ShowIf("@_gameModeId == Guid.Empty.ToString()")]
        private void GenerateId()
        {
            if (_gameModeId != Guid.Empty.ToString())
            {
                return;
            }

            _gameModeId = Guid.NewGuid().ToString();
        }
#endif
    }
}
