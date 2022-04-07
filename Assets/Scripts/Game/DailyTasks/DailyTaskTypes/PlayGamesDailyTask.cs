using UnityEngine;

namespace Sufka.Game.DailyTasks.DailyTaskTypes
{
    
    [CreateAssetMenu(fileName = "Play Games", menuName = "Sufka/Daily Tasks/Types/Play Games", order = 0)]
    public class PlayGamesDailyTask : DailyTask
    {
        public override string Description => $"Rozegraj {_targetAmount} gier";
    }
}
