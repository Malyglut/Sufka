using UnityEngine;

namespace Sufka.Game.Achievements.AchievementTypes
{
    [CreateAssetMenu(fileName = "Unlocked Color Schemes", menuName = "Sufka/Achievements/Types/Unlocked Color Schemes",
                     order = 0)]
    public class UnlockedColorSchemesAchievement : Achievement
    {
        public override string Description =>
            _targetAmount > 1 ? $"Odblokuj {_targetAmount} kolorów" : $"Odblokuj {_targetAmount} kolor";
    }
}
