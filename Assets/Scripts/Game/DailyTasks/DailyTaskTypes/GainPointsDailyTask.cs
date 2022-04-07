using Sufka.Game.Utility;
using UnityEngine;

namespace Sufka.Game.DailyTasks.DailyTaskTypes
{
    [CreateAssetMenu(fileName = "Gain Points", menuName = "Sufka/Daily Tasks/Types/Gain Points", order = 0)]
    public class GainPointsDailyTask : DailyTask
    {
        public override string Description => GenerateDescription();

        private string GenerateDescription()
        {
            var pointsString = PolishTextUtility.GetProperPointsString(_targetAmount);

            return $"Zdobądź {_targetAmount} {pointsString} za odgadnięte słówka";
        }
    }
}
