using UnityEngine;

namespace Sufka.Game.Achievements
{
    [CreateAssetMenu(fileName = "New Achievement Type", menuName = "Sufka/Achievements/Achievement Type", order = 0)]
    public class AchievementType : ScriptableObject
    {
        [SerializeField]
        private int _achievementListOrder = 0;

        public int AchievementListOrder => _achievementListOrder;
    }
}
