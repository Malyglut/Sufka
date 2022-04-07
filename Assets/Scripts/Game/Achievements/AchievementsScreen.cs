using System;
using System.Collections.Generic;
using System.Linq;
using Sufka.Game.Achievements.AchievementTypes;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    public class AchievementsScreen : MonoBehaviour
    {
        public event Action OnOpen = EventUtility.Empty;
        
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

            SortDisplays();
        }

        private void SortDisplays()
        {
            var sortedDisplays = _displays
                                .OrderBy(display => display.Achievement.Type.ListOrder).ThenBy(display =>
                                     display.Achievement is GuessedWordsAchievement
                                         guessedWordsAchievement &&
                                     guessedWordsAchievement.GameMode != null ?
                                         guessedWordsAchievement.GameMode.OrderInList :
                                         0)
                                .ThenBy(display => display.Achievement.TargetAmount).ToList();

            for (int i = 0; i < sortedDisplays.Count; i++)
            {
                sortedDisplays[i].transform.SetSiblingIndex(i);
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
            
            OnOpen.Invoke();
        }
    }
}
