using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    [CreateAssetMenu(fileName = "New Achievement Database", menuName = "Sufka/Achievements/Achievement Database", order = 0)]
    public class AchievementDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Achievement> _achievements = new List<Achievement>();

        public List<Achievement> Achievements => _achievements;
    }
}
