using UnityEngine;

namespace Sufka.Game.DailyTasks.DailyTaskTypes
{
    [CreateAssetMenu(fileName = "Use Hints", menuName = "Sufka/Daily Tasks/Types/Use Hints", order = 0)]
    public class UseHintsDailyTask : DailyTask
    {
        public override string Description => $"Wykorzystaj {_targetAmount} podpowiedz.";
    }
}
