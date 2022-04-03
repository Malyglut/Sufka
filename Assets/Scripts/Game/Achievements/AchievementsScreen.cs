using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    public class AchievementsScreen : MonoBehaviour
    {

        
        [SerializeField]
        private AchievementDisplay _displayPrefab;
        
        [SerializeField]
        private Transform _parent;

        private Dictionary<Achievement, AchievementDisplay> _achievementDisplays =
            new Dictionary<Achievement, AchievementDisplay>();

        public void Initialize(List<Achievement> achievements)
        {
            foreach (var achievement in achievements)
            {
                if (!achievement.Hidden && achievement.PrecedingAchievement == null /*|| preceding is unlocked*/)
                {
                    var display = Instantiate(_displayPrefab, _parent);
                    display.Initialize(achievement);

                    _achievementDisplays.Add(achievement, display);
                }
            }
        }
    }
}
