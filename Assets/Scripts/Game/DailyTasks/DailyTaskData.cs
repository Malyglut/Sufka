using System;

namespace Sufka.Game.DailyTasks
{
    [Serializable]
    public class DailyTaskData
    {
        public string dailyTaskId;
        public int currentAmount;
        public bool rewardCollected;

        public DailyTaskData(DailyTask task)
        {
            dailyTaskId = task.DailyTaskId;
            currentAmount = task.CurrentAmount;
            rewardCollected = task.RewardCollected;
        }
    }
}
