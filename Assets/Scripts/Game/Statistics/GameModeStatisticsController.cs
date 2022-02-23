using System.Collections.Generic;
using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Statistics
{
    public class GameModeStatisticsController : MonoBehaviour
    {
        [SerializeField]
        private GameMode _gameMode;

        [SerializeField]
        private List<StatisticDisplay> _displaysToHide = new List<StatisticDisplay>();

        [SerializeField]
        private int _lastPageIdx;
        
        public GameMode GameMode => _gameMode;
        public int LastPageIdx => _lastPageIdx;

        public void Prepare()
        {
            foreach (var display in _displaysToHide)
            {
                display.gameObject.SetActive(false);
            }
        }

        public void Revert()
        {
            foreach (var display in _displaysToHide)
            {
                display.gameObject.SetActive(true);
            }
        }
    }
}
