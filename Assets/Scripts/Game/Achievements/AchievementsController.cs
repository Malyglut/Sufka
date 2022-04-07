using System;
using System.Collections.Generic;
using System.Linq;
using Sufka.Game.Achievements.AchievementTypes;
using Sufka.Game.Colors;
using Sufka.Game.GameFlow;
using Sufka.Game.TaskTypes;
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
        private TaskTypeDatabase _taskTypeDatabase;

        private readonly Dictionary<TaskType, List<Achievement>> _achievementTypes =
            new Dictionary<TaskType, List<Achievement>>();
        public IEnumerable<Achievement> CompletedAchievements =>
            _database.Achievements.Where(achievement => achievement.Completed);

#if UNITY_EDITOR
        [Button]
#endif
        public void Initialize(List<string> completedAchievementIds)
        {
#if UNITY_EDITOR
            _database.ResetProgress();
#endif

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
                if (achievement.Type == _taskTypeDatabase.GuessedWords)
                {
                    UpdateGuessedWordsAchievement(achievement);
                }
                else if (achievement.Type == _taskTypeDatabase.HintsUsed)
                {
                    UpdateHintsUsedAchievement(achievement);
                }
                else if (achievement.Type == _taskTypeDatabase.UnlockedColorSchemes)
                {
                    UpdateUnlockedColorSchemesAchievement(achievement);
                }
                else if (achievement.Type == _taskTypeDatabase.Tutorial)
                {
                    UpdateTutorialAchievement(achievement);
                }
                else if (achievement.Type == _taskTypeDatabase.CompletedDailyTasks)
                {
                    UpdateCompletedDailyTasksAchievement(achievement);
                }
            }
        }

        private void UpdateCompletedDailyTasksAchievement(Achievement achievement)
        {
            achievement.UpdateCurrentAmount(_gameController.CompletedDailyTasks);
        }

        private void UpdateUnlockedColorSchemesAchievement(Achievement achievement)
        {
            var unlockedColorsCount = _gameController.UnlockedColorIds.Count - 1;
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

        private void UpdateTutorialAchievement(Achievement achievement)
        {
            achievement.UpdateCurrentAmount(_gameController.TutorialCompletionsCount);
        }

        public void HandleWordGuessed(GameMode gameMode)
        {
            HandleAchievementProgressUpdated(_taskTypeDatabase.GuessedWords, UpdateGuessedWordsAchievement);
        }

        public void HandleHintUsed()
        {
            HandleAchievementProgressUpdated(_taskTypeDatabase.HintsUsed, UpdateHintsUsedAchievement);
        }

        public void HandleColorUnlocked()
        {
            HandleAchievementProgressUpdated(_taskTypeDatabase.UnlockedColorSchemes, UpdateUnlockedColorSchemesAchievement);
        }

        public void HandleTutorialCompleted()
        {
            HandleAchievementProgressUpdated(_taskTypeDatabase.Tutorial, UpdateTutorialAchievement);
        }

        public void HandleDailyTaskCompleted()
        {
            HandleAchievementProgressUpdated(_taskTypeDatabase.CompletedDailyTasks, UpdateCompletedDailyTasksAchievement);
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
    }
}
