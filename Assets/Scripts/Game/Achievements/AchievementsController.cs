using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sufka.Game.GameFlow;
using Sufka.Game.Statistics;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    public class AchievementsController : MonoBehaviour
    {
        [SerializeField]
        private GameController _gameController;
        
        [SerializeField]
        private AchievementDatabase _database;

        [SerializeField]
        private AchievementsScreen _achievementsScreen;

        [SerializeField]
        private AchievementType _guessedWords;

        private Dictionary<AchievementType, List<Achievement>> _achievementTypes =
            new Dictionary<AchievementType, List<Achievement>>();
        public IEnumerable<Achievement> CompletedAchievements =>
            _database.Achievements.Where(achievement => achievement.Completed);

        public void Initialize()
        {
            foreach (var achievement in _database.Achievements)
            {
                if (!_achievementTypes.ContainsKey(achievement.Type))
                {
                    _achievementTypes.Add(achievement.Type, new List<Achievement>());
                }

                _achievementTypes[achievement.Type].Add(achievement);
                UpdateCurrentAmount(achievement);
            }
            
            _achievementsScreen.RefreshAvailableAchievements(_database.Achievements);
        }

        private void UpdateCurrentAmount(Achievement achievement)
        {
            if(!achievement.Completed)
            {
                UpdateGuessedWordsAchievement(achievement);
            }
        }

        private void UpdateGuessedWordsAchievement(Achievement achievement)
        {
            if (achievement.Type == _guessedWords)
            {
                var guessedWordsAchievement = (GuessedWordsAchievement) achievement;

                WordStatistics wordStatistics;

                if (guessedWordsAchievement.GameMode == null)
                {
                    wordStatistics = _gameController.GetOverallStatistics();
                }
                else
                {
                    wordStatistics = _gameController.GetStatistics(guessedWordsAchievement.GameMode);
                }

                achievement.UpdateCurrentAmount(wordStatistics.guessedWords);
            }
        }

        public void HandleWordGuessed(GameMode gameMode)
        {
            foreach (var achievement in _achievementTypes[_guessedWords])
            {
                UpdateGuessedWordsAchievement(achievement);
            }
        }
    }
}
