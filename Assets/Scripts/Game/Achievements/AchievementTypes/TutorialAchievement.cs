using UnityEngine;

namespace Sufka.Game.Achievements.AchievementTypes
{
    [CreateAssetMenu(fileName = "New Guessed Words Achievement", menuName = "Sufka/Achievements/Types/Tutorial",
                     order = 0)]
    public class TutorialAchievement : Achievement
    {
        public override string Description =>
            _targetAmount > 1 ? $"Ukończ samouczek {_targetAmount} razy" : "Ukończ samouczek";
    }
}
