using UnityEngine;

namespace Sufka.Game.Achievements.AchievementTypes
{
    [CreateAssetMenu(fileName = "Hints Used", menuName = "Sufka/Achievements/Types/Hints Used", order = 0)]
    public class HintsUsedAchievement : Achievement
    {
        public override string Description => _targetAmount > 1 ?
                                                  $"Wykorzystaj {_targetAmount} podpowiedzi" :
                                                  $"Wykorzystaj {_targetAmount} podpowiedź";
    }
}
