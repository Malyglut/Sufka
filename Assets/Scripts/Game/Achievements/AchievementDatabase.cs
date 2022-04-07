using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    [CreateAssetMenu(fileName = "New Achievement Database", menuName = "Sufka/Achievements/Achievement Database",
                     order = 0)]
    public class AchievementDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Achievement> _achievements = new List<Achievement>();

        public List<Achievement> Achievements => _achievements;

        public void ResetProgress()
        {
            foreach (var achievement in _achievements)
            {
                achievement.UpdateCurrentAmount(0);
            }
        }

#if UNITY_EDITOR
        
        [Button]
        public void CheckForDuplicates()
        {
            foreach (var achievement in _achievements)
            {
                var filteredAchievements = new List<Achievement>(_achievements);
                filteredAchievements.Remove(achievement);

                if (filteredAchievements.Any(duplicateCandidate => duplicateCandidate.AchievementId == achievement.AchievementId))
                {
                    Debug.Log($"DUPLICATE ACHIEVEMENT ID FOUND {achievement}");
                    break;
                }
                
            }

            Debug.Log("NO DUPLICATE ACHIEVEMENT IDS FOUND");
        }
#endif

    }
}
