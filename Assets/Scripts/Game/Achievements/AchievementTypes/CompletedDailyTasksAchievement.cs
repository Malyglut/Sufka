using Sufka.Game.Utility;
using UnityEngine;

namespace Sufka.Game.Achievements.AchievementTypes
{
    [CreateAssetMenu(fileName = "Completed Daily Tasks Achievement", menuName = "Sufka/Achievements/Types/Completed Daily Tasks",
                     order = 0)]
    public class CompletedDailyTasksAchievement : Achievement
    {
        public override string Description=> GenerateDescription();

        private string GenerateDescription()
        {
            var tasksString = PolishTextUtility.GetProperTasksString(_targetAmount);

            return $"Wykonaj {_targetAmount} {tasksString}";
        }
    }
}
