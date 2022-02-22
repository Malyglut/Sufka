using System;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    [CreateAssetMenu(fileName = "Current State", menuName = "Sufka/Current State", order = 0)]
    public class CurrentState : ScriptableObject
    {
        public event Action OnAvailableHintsUpdated;

        private int _availableHints;
        
        public int Score;
        public int AvailableHints
        {
            get
            {
                return _availableHints;
            }
            set
            {
                _availableHints = value;
                OnAvailableHintsUpdated?.Invoke();
            }
        }
    }
}
