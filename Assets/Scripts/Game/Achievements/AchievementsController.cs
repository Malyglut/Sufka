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

        public void Initialize()
        {
            foreach (var achievement in _database.Achievements)
            {
                UpdateCurrentAmount(achievement);
            }
            
            _achievementsScreen.Initialize(_database.Achievements);
        }

        private void UpdateCurrentAmount(Achievement achievement)
        {
            UpdateGuessedWordsAchievement(achievement);
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
            
        }
    }
}
