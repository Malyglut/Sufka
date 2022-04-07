using System;
using System.Collections.Generic;
using System.Linq;
using Sufka.Game.Colors;
using Sufka.Game.GameFlow;
using Sufka.Game.Utility;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;

#endif

namespace Sufka.Game.Achievements
{
    public class AchievementsController : MonoBehaviour
    {
        public event Action<Achievement> OnAchievementCompleted = EventUtility.Empty;

        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private AchievementDatabase _database;

        [SerializeField]
        private AchievementsScreen _achievementsScreen;

        [SerializeField]
        private TaskType _guessedWords;

        [SerializeField]
        private TaskType _hintsUsed;

        [SerializeField]
        private TaskType _unlockedColorSchemes;

        private readonly Dictionary<TaskType, List<Achievement>> _achievementTypes =
            new Dictionary<TaskType, List<Achievement>>();
        public IEnumerable<Achievement> CompletedAchievements =>
            _database.Achievements.Where(achievement => achievement.Completed);

#if UNITY_EDITOR
        [Button]
#endif
        public void Initialize(List<string> completedAchievementIds)
        {
            _database.ResetProgress();

            foreach (var achievement in _database.Achievements)
            {
                if (!_achievementTypes.ContainsKey(achievement.Type))
                {
                    _achievementTypes.Add(achievement.Type, new List<Achievement>());
                }

                if (completedAchievementIds != null && completedAchievementIds.Contains(achievement.AchievementId))
                {
                    achievement.Complete();
                }
                else
                {
                    _achievementTypes[achievement.Type].Add(achievement);
                    UpdateCurrentAmount(achievement);
                }
            }

            _achievementsScreen.RefreshAvailableAchievements(_database.Achievements);
        }

        private void UpdateCurrentAmount(Achievement achievement)
        {
            if (!achievement.Completed)
            {
                if (achievement.Type == _guessedWords)
                {
                    UpdateGuessedWordsAchievement(achievement);
                }
                else if (achievement.Type == _hintsUsed)
                {
                    UpdateHintsUsedAchievement(achievement);
                }
                else if (achievement.Type == _unlockedColorSchemes)
                {
                    UpdateUnlockedColorSchemesAchievement(achievement);
                }
            }
        }

        private void UpdateUnlockedColorSchemesAchievement(Achievement achievement)
        {
            var unlockedColorsCount = _gameController.UnlockedColorIds.Count;
            achievement.UpdateCurrentAmount(unlockedColorsCount);
        }

        private void UpdateHintsUsedAchievement(Achievement achievement)
        {
            var overallStatistics = _gameController.GetOverallStatistics();
            achievement.UpdateCurrentAmount(overallStatistics.hintsUsed);
        }

        private void UpdateGuessedWordsAchievement(Achievement achievement)
        {
            var guessedWordsAchievement = (GuessedWordsAchievement) achievement;

            var wordStatistics = guessedWordsAchievement.GameMode == null ?
                                     _gameController.GetOverallStatistics() :
                                     _gameController.GetStatistics(guessedWordsAchievement.GameMode);

            achievement.UpdateCurrentAmount(wordStatistics.guessedWords);
        }

        public void HandleWordGuessed(GameMode gameMode)
        {
            HandleAchievementProgressUpdated(_guessedWords, UpdateGuessedWordsAchievement);
        }

        public void HandleHintUsed()
        {
            HandleAchievementProgressUpdated(_hintsUsed, UpdateHintsUsedAchievement);
        }

        private void HandleAchievementProgressUpdated(TaskType type, Action<Achievement> updateAction)
        {
            foreach (var achievement in _achievementTypes[type])
            {
                if (achievement.Completed)
                {
                    continue;
                }

                updateAction.Invoke(achievement);

                if (achievement.Completed)
                {
                    _achievementsScreen.RefreshAvailableAchievements(_database.Achievements);
                    OnAchievementCompleted.Invoke(achievement);
                }
            }
        }

        public void HandleColorUnlocked()
        {
            HandleAchievementProgressUpdated(_unlockedColorSchemes, UpdateUnlockedColorSchemesAchievement);
        }
    }
}
