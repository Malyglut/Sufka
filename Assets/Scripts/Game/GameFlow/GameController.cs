using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sufka.Game.Achievements;
using Sufka.Game.Ads;
using Sufka.Game.Analytics;
using Sufka.Game.Colors;
using Sufka.Game.DailyTasks;
using Sufka.Game.InGameNotifications;
using Sufka.Game.MainMenu;
using Sufka.Game.Persistence;
using Sufka.Game.Popup;
using Sufka.Game.Statistics;
using Sufka.Game.Tutorial;
using Sufka.Game.Unlocks;
using Sufka.Game.Utility;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Sufka.Game.GameFlow
{
    public class GameController : MonoBehaviour, IUnityAdsListener
    {
        private const int HINTS_PER_AD = 10;
        private const int HINTS_PER_HINT_REWARD = 1;

        [SerializeField]
        private ColorSchemeDatabase _colorSchemeDatabase;

        [SerializeField]
        private AvailableGameModes _availableGameModes;

        [SerializeField]
        private MainMenuController _mainMenu;

        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private PopupController _popup;

        [SerializeField]
        private TutorialController _tutorial;

        [SerializeField]
        private AchievementsController _achievements;

        [SerializeField]
        private NotificationController _notifications;

        [SerializeField]
        private DailyTaskController _dailyTasks;

        private readonly StatisticsController _statistics = new StatisticsController();
        private readonly AdsController _ads = new AdsController();

        private TutorialController _tutorialInstance;
        private GameInProgressSaveData _gameInProgressSaveData;

        private SaveData _saveData;
        private int _pointsForAd;
        private bool _gameInProgress;

        public ColorScheme SelectedColorScheme =>
            _colorSchemeDatabase.ColorSchemes.First(color => color.ColorId == _saveData.selectedColorSchemeId);
        public List<string> UnlockedGameModeIds => _saveData.unlockedGameModeIds;
        public List<string> UnlockedColorIds => _saveData.unlockedColorIds;
        public int PointsSpent => _saveData.pointsSpentOnUnlocks;
        public int PointsSpentOnColors => _saveData.pointsSpentOnColors;
        public int ColorsUnlocked => _saveData.unlockedColorIds.Count;
        public int Score => _saveData.score;
        public int AvailableHints => _saveData.availableHints;
        public bool HintUsed => _gameInProgressSaveData.hintUsed;
        public int TutorialCompletionsCount => _saveData.tutorialCompletionsCount;
        public int CompletedDailyTasks => _saveData.completedDailyTasksCount;

        private void StartTutorial()
        {
            _mainMenu.gameObject.SetActive(false);
            _tutorialInstance = Instantiate(_tutorial);
            _tutorialInstance.OnComplete += FinalizeTutorial;
            _mainMenu.RefreshColors();
        }

        private void FinalizeTutorial()
        {
            Destroy(_tutorialInstance.gameObject);
            _mainMenu.gameObject.SetActive(true);

            _saveData.tutorialCompleted = true;
            _saveData.tutorialCompletionsCount++;
            _achievements.HandleTutorialCompleted();
            SaveGame();
        }

        private void Start()
        {
            LoadGame();
            InitializePlayArea();
            InitializeMainMenu();
            InitializeAchievements();
            InitializeDailyTasks();

            _popup.Initialize();

            if (!_saveData.tutorialCompleted)
            {
                StartTutorial();
            }
        }

        private void InitializeDailyTasks()
        {
            if (_saveData.previousDailyTasks == null)
            {
                _saveData.previousDailyTasks = new List<string>();
            }

            if (_saveData.dailyTasksData == null)
            {
                _saveData.dailyTasksData = new List<DailyTaskData>();
            }

            if (_saveData.nextDailyTasksGenerationDateTime == null)
            {
                _saveData.nextDailyTasksGenerationDateTime = new DateTimeSaveData(DateTime.Now);
            }

            var dateTimeData = _saveData.nextDailyTasksGenerationDateTime;

            var nextDailyTasksDateTime = new DateTime(
                                                      dateTimeData.year, 
                                                      dateTimeData.month,
                                                      dateTimeData.day,
                                                      dateTimeData.hour,
                                                      dateTimeData.minute,
                                                      dateTimeData.second);

            _dailyTasks.OnDailyTasksGenerated += SaveDailyTasksData;
            _dailyTasks.OnDailyTaskCompleted += HandleDailyTaskCompleted;
            _dailyTasks.OnDailyTasksUpdated += UpdateDailyTaskProgress;
            _dailyTasks.OnRewardClaimed += ClaimDailyTaskReward;
            
            _dailyTasks.Initialize(nextDailyTasksDateTime, _saveData.dailyTasksData, _saveData.previousDailyTasks);
        }

        private void ClaimDailyTaskReward(int pointsReward, List<DailyTask> currentTasks)
        {
            _saveData.score += pointsReward;
            _saveData.pointsGainedFromTasks += pointsReward;
            _playArea.RefreshPoints(pointsReward);

            UpdateDailyTaskProgress(currentTasks);
            
            SaveGame();
        }

        private void HandleDailyTaskCompleted(DailyTask dailyTask)
        {
            _notifications.ShowDailyTaskNotification(dailyTask);
            _saveData.completedDailyTasksCount++;
            _achievements.HandleDailyTaskCompleted();
            SaveGame();
        }

        private void SaveDailyTasksData(List<DailyTask> currentDailyTasks, List<string> previousTasksIds, DateTime nextDailyTaskGeneration)
        {
            UpdateDailyTaskProgress(currentDailyTasks);
            _saveData.previousDailyTasks = previousTasksIds;
            _saveData.nextDailyTasksGenerationDateTime = new DateTimeSaveData(nextDailyTaskGeneration);
            
            SaveGame();
        }

        private void UpdateDailyTaskProgress(List<DailyTask> currentDailyTasks)
        {
            var taskData= new List<DailyTaskData>();

            foreach (var task in currentDailyTasks)
            {
                taskData.Add(new DailyTaskData(task));
            }

            _saveData.dailyTasksData = taskData;
        }

        private void InitializeAchievements()
        {
            _achievements.OnAchievementCompleted += HandleAchievementCompleted;
            _achievements.Initialize(_saveData.completedAchievements);

            if (_saveData.completedAchievements == null)
            {
                _saveData.completedAchievements = new List<string>();

                foreach (var achievement in _achievements.CompletedAchievements)
                {
                    _saveData.completedAchievements.Add(achievement.AchievementId);
                }
            }
        }

        private void InitializeMainMenu()
        {
            _mainMenu.OnRequestGameStart += StartGame;
            _mainMenu.OnContinueRequested += ContinueGame;
            _mainMenu.OnRequestUnlockColorScheme += ShowUnlockColorSchemePopup;
            _mainMenu.OnNotifyColorSchemeChanged += UpdateSelectedColorScheme;
            _mainMenu.OnRequestUnlockGameMode += ShowUnlockGameModePopup;
            _mainMenu.OnTutorialRequested += StartTutorial;
            
            _mainMenu.Initialize();

            ShowMainMenu();
        }

        private void InitializePlayArea()
        {
            _playArea.OnHintUsed += HandleHintUsed;
            _playArea.OnWordGuessed += HandleWordGuessed;
            _playArea.OnPointsAwarded += IncreaseScore;
            _playArea.OnHintAdRequested += ShowHintPopup;
            _playArea.OnBackToMenuPopupRequested += ShowBackToMenuPopup;
            _playArea.OnGameProgressUpdated += SaveGameProgress;
            _playArea.OnRoundOver += HandleRoundOver;
            _playArea.OnLetterStatisticsUpdated += HandleLetterStatisticsUpdated;
            _playArea.OnRoundStarted += UpdateGameInProgress;
            _playArea.OnDailyTasksScreenRequested += ShowDailyTasksScreen;
        }

        private void ShowDailyTasksScreen()
        {
            _mainMenu.ShowDailyTasksScreenInGame();
        }

        private void HandleAchievementCompleted(Achievement achievement)
        {
            _saveData.completedAchievements.Add(achievement.AchievementId);
            _notifications.ShowAchievementNotification(achievement);
            SaveGame();
        }

        private void UpdateGameInProgress()
        {
            _gameInProgress = true;
            SaveGameProgress();
        }

        private void HandleLetterStatisticsUpdated(
            int correctLetters, int correctSpotLetters, int typedLetters, int removedLetters)
        {
            var statistics = GetStatistics(_playArea.GameMode);

            statistics.correctLetters += correctLetters;
            statistics.lettersInCorrectSpot += correctSpotLetters;
            statistics.typedLetters += typedLetters;
            statistics.removedLetters += removedLetters;

            SaveGame();
        }

        private void HandleRoundOver()
        {
            _gameInProgress = false;

            _saveData.wordsUntilHintReward--;

            _dailyTasks.HandleGamePlayed();
            
            if (_saveData.wordsUntilHintReward <= 0)
            {
                AnalyticsEvents.HintForPlaying(_saveData.availableHints);
                _saveData.availableHints += HINTS_PER_HINT_REWARD;
                _saveData.ResetWordsUntilHintReward();
                _playArea.RefreshHints();
            }

            SaveGame();
            SaveGameProgress();

            AnalyticsEvents.WordNotGuessed(_playArea.GameMode.Name, _playArea.TargetWord.fullWord);
        }

        private void ShowUnlockGameModePopup(GameMode gameMode)
        {
            _popup.UnlockGameModePopup(gameMode.Name, gameMode.UnlockCost, Score, () => UnlockGameMode(gameMode));
            AnalyticsEvents.GameModePopup(gameMode.Name, Score);
        }

        private void UnlockGameMode(GameMode gameMode)
        {
            _saveData.score -= gameMode.UnlockCost;
            _saveData.unlockedGameModeIds.Add(gameMode.GameModeId);
            _saveData.pointsSpentOnUnlocks += gameMode.UnlockCost;
            SaveGame();

            _mainMenu.RefreshGameModes();

            AnalyticsEvents.GameModeUnlocked(gameMode.Name);
        }

        private void UpdateSelectedColorScheme(ColorScheme colorScheme)
        {
            _saveData.selectedColorSchemeId = colorScheme.ColorId;
            SaveGame();

            AnalyticsEvents.ColorSchemeSelected(colorScheme.Name);
        }

        private void ShowUnlockColorSchemePopup(ColorScheme colorScheme)
        {
            _popup.UnlockColorSchemePopup(colorScheme.ColoredString(), colorScheme.UnlockCost, Score,
                                          () => UnlockColorScheme(colorScheme));

            AnalyticsEvents.ColorSchemeUnlockPopup(colorScheme.Name, Score);
        }

        private void UnlockColorScheme(ColorScheme colorScheme)
        {
            _saveData.score -= colorScheme.UnlockCost;
            _saveData.unlockedColorIds.Add(colorScheme.ColorId);

            _saveData.pointsSpentOnColors += colorScheme.UnlockCost;
            _saveData.pointsSpentOnUnlocks += colorScheme.UnlockCost;

            _achievements.HandleColorUnlocked();
            
            SaveGame();

            _mainMenu.RefreshColorSchemes();

            AnalyticsEvents.ColorSchemeUnlocked(colorScheme.Name);
        }

        private void SaveGameProgress()
        {
            var gameModeIdx = _availableGameModes.GameModes.IndexOf(_playArea.GameMode);
            _gameInProgressSaveData.Update(_gameInProgress, gameModeIdx, _playArea);

            SaveSystem.SaveGameInProgress(_gameInProgressSaveData);
        }

        private void ShowBackToMenuPopup()
        {
            _popup.BackToMenuPopup(ShowMainMenu);
        }

        private void ShowMainMenu()
        {
            _playArea.Hide();
            _mainMenu.ShowTitleScreen(_gameInProgressSaveData.gameInProgress);
        }

        private void ShowHintPopup()
        {
            _popup.ShowHintPopup(HINTS_PER_AD, HandleHintAdDeclined, _ads.PlayHintAd);

            AnalyticsEvents.HintPopup();
        }

        private void HandleHintAdDeclined()
        {
            AnalyticsEvents.HintAdDeclined();
        }

        private void IncreaseScore(int points)
        {
            _saveData.score += points;
            _saveData.bonusPointsReward += points;

            var gameMode = _playArea.GameMode == null ? _availableGameModes.GameModes[0] : _playArea.GameMode;
            _statistics.HandlePointsGained(points, gameMode, _availableGameModes);
            _dailyTasks.HandlePointsGained(points);

            SaveGame();
        }

        private void HandleWordGuessed(int attempt)
        {
            HandleRoundOver();

            _statistics.HandleWordGuessed(_playArea.GameMode, attempt, _availableGameModes);
            _achievements.HandleWordGuessed(_playArea.GameMode);
            _dailyTasks.HandleWordGuessed();

            _saveData.wordsUntilBonusPointsReward--;

            if (_saveData.wordsUntilBonusPointsReward <= 0)
            {
                _pointsForAd = _saveData.bonusPointsReward;
                _saveData.ResetWordsUntilBonusPointsReward();

                _popup.BonusPointsPopup(_saveData.wordsUntilBonusPointsReward, _pointsForAd, HandlePointsAdDeclined,
                                        _ads.PlayBonusPointsAd);
            }

            SaveGame();

            AnalyticsEvents.WordGuessed(_playArea.GameMode.Name, attempt, _playArea.TargetWord.fullWord);
        }

        private void HandlePointsAdDeclined()
        {
            AnalyticsEvents.PointsAdDeclined(_pointsForAd);
            _pointsForAd = 0;
        }

        private void HandleHintUsed()
        {
            AnalyticsEvents.HintUsed(_playArea.GameMode.Name, _playArea.TargetWord.fullWord, _playArea.CurrentAttempt,
                                     _saveData.availableHints);

            _saveData.availableHints--;
            _statistics.HandleHintUsed(_playArea.GameMode, _availableGameModes);
            _achievements.HandleHintUsed();
            _dailyTasks.HandleHintUsed();
            SaveGame();
        }

        private void StartGame(GameMode gameMode)
        {
            InitializeAds();

            _playArea.StartGame(gameMode);

            AnalyticsEvents.NewGame(gameMode.Name);
        }

        private void ContinueGame()
        {
            InitializeAds();

            var gameMode = _availableGameModes.GameModes[_gameInProgressSaveData.gameModeIdx];
            _playArea.StartGame(gameMode, _gameInProgressSaveData);

            AnalyticsEvents.ContinueGame(gameMode.Name);
        }

        private void InitializeAds()
        {
            _ads.Initialize(this);
        }

        private void SaveGame()
        {
            _saveData.wordStatistics = _statistics.WordStatistics;
            SaveSystem.SaveGame(_saveData);
        }

        private void LoadGame()
        {
            _saveData = SaveSystem.LoadGame();

            _statistics.Load(_saveData, _availableGameModes);

            _gameInProgressSaveData = SaveSystem.LoadGameInProgress();

            if (_saveData.unlockedColorIds == null)
            {
                _saveData.unlockedColorIds = new List<string>();

                for (var i = 0; i < _saveData.unlockedColors.Count; i++)
                {
                    if (_saveData.unlockedColors[i])
                    {
                        _saveData.unlockedColorIds.Add(_colorSchemeDatabase.ColorSchemes[i].ColorId);
                    }
                }
            }

            if (_saveData.unlockedGameModeIds == null)
            {
                _saveData.unlockedGameModeIds = new List<string>();

                for (var i = 0; i < _saveData.unlockedGameModes.Count; i++)
                {
                    if (_saveData.unlockedGameModes[i])
                    {
                        _saveData.unlockedGameModeIds.Add(_availableGameModes.GameModes[i].GameModeId);
                    }
                }
            }

            if (_saveData.selectedColorSchemeId == null)
            {
                _saveData.selectedColorSchemeId =
                    _colorSchemeDatabase.ColorSchemes[_saveData.selectedColorSchemeIdx].ColorId;
            }
        }

        public WordStatistics GetStatistics(GameMode gameMode)
        {
            return _statistics.GetStatistics(gameMode, _availableGameModes);
        }

        public void OnUnityAdsReady(string placementId)
        {
            //do nothing
        }

        public void OnUnityAdsDidError(string message)
        {
            //do nothing
        }

        public void OnUnityAdsDidStart(string placementId)
        {
#if UNITY_EDITOR
            Debug.Log($"AD STARTED {placementId}");
#endif
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
#if UNITY_EDITOR
            Debug.Log($"AD FINISHED {placementId}");
#endif

            if (showResult == ShowResult.Finished)
            {
                if (placementId == _ads.HintsAdId)
                {
                    _saveData.availableHints += HINTS_PER_AD;
                    _playArea.RefreshHints();
                    SaveGame();
                    AnalyticsEvents.HintAdWatched();
                }
                else if (placementId == _ads.BonusPointsAdId)
                {
                    AnalyticsEvents.PointsAdWatched(_pointsForAd);
                    _saveData.score += _pointsForAd;
                    _playArea.RefreshPoints(_pointsForAd);
                    _statistics.HandlePointsGained(_pointsForAd, _playArea.GameMode, _availableGameModes);
                    _pointsForAd = 0;
                    SaveGame();
                }
            }
        }

        public WordStatistics GetOverallStatistics()
        {
            return _statistics.GetOverallStatistics();
        }

#if UNITY_EDITOR
        [Button]
        private void AddPoints(int points)
        {
            IncreaseScore(points);
        }

        [Button]
        private void IncreaseHintsUsed(int amount)
        {
            _statistics.WordStatistics[0].hintsUsed += amount;
            SaveGame();
        }
#endif
    }
}
