using System;
using Sirenix.OdinInspector;
using Sufka.Game.TaskTypes;
using UnityEngine;

namespace Sufka.Game.DailyTasks
{
    public abstract class DailyTask : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private string _dailyTaskId = Guid.Empty.ToString();
        
        [SerializeField]
        private TaskType _type;

        [SerializeField]
        protected int _targetAmount;

        [SerializeField]
        private int _pointsReward;
        
        public abstract string Description { get; }
        
        public bool RewardCollected { get; private set; }
        public string DailyTaskId => _dailyTaskId;
        public int CurrentAmount { get; private set; }

        public TaskType Type => _type;
        public int TargetAmount => _targetAmount;
        public bool Completed => CurrentAmount >= _targetAmount;

        public void IncreaseCurrentAmount(int amount)
        {
            CurrentAmount += amount;
        }
        
        public void Load(DailyTaskData taskData)
        {
            CurrentAmount = taskData.currentAmount;
            RewardCollected = taskData.rewardCollected;
        }

#if UNITY_EDITOR

        [Button, ShowIf("@_dailyTaskId == Guid.Empty.ToString()")]
        private void GenerateId()
        {
            if (_dailyTaskId != Guid.Empty.ToString())
            {
                return;
            }

            _dailyTaskId = Guid.NewGuid().ToString();
        }
#endif
    }
}
