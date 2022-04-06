using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    public class AchievementsScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _achievementCount;
        
        [SerializeField]
        private AchievementDisplay _displayPrefab;
        
        [SerializeField]
        private Transform _parent;

        private readonly Dictionary<Achievement, AchievementDisplay> _achievementDisplays =
            new Dictionary<Achievement, AchievementDisplay>();

        private readonly List<AchievementDisplay> _displays = new List<AchievementDisplay>();

        public void RefreshAvailableAchievements(List<Achievement> achievements)
        {
            foreach (var achievement in achievements)
            {
                if (!achievement.Hidden && (achievement.PrecedingAchievement == null || achievement.PrecedingAchievement.Completed))
                {
                    var display = Instantiate(_displayPrefab, _parent);
                    display.Initialize(achievement);

                    _achievementDisplays.Add(achievement, display);
                    _displays.Add(display);
                }
            }
        }

        public void RefreshAchievementProgress()
        {
            var completedAchievementCount = _achievementDisplays.Keys.Count(achievement => achievement.Completed);
            _achievementCount.SetText(completedAchievementCount.ToString());
            
            foreach (var achievementDisplay in _achievementDisplays.Values)
            {
                achievementDisplay.Refresh();
            }
        }
    }
}
