using UnityEngine;

namespace Sufka.Game.Achievements
{
    [CreateAssetMenu(fileName = "Hints Used", menuName = "Sufka/Achievements/Types/Hints Used", order = 0)]
    public class HintsUsedAchievement : Achievement
    {
        public override string Description => $"Użyj {_targetAmount} podpowiedzi";
    }
}
