using System.Collections.Generic;
using System.Linq;
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
            var sortedAchievements = achievements.OrderBy(achievement => achievement.Type.AchievementListOrder)
                                                 .ThenBy(achievement => achievement.TargetAmount);
            
            foreach (var achievement in sortedAchievements)
            {
                if (_achievementDisplays.ContainsKey(achievement))
                {
                    continue;
                }

                if (achievement.Hidden && !achievement.Completed)
                {
                    continue;
                }

                if (achievement.PrecedingAchievement == null || achievement.PrecedingAchievement.Completed)
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
