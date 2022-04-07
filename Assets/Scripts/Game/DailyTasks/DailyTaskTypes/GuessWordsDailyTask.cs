using UnityEngine;

namespace Sufka.Game.DailyTasks.DailyTaskTypes
{
    [CreateAssetMenu(fileName = "Guess Words", menuName = "Sufka/Daily Tasks/Types/Guess Words", order = 0)]
    public class GuessWordsDailyTask : DailyTask
    {
        public override string Description => $"Odgadnij {_targetAmount} słówek";
    }
}
