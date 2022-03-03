using System.Collections.Generic;
using Sufka.Game.Ads;
using Sufka.Game.Colors;
using Sufka.Game.MainMenu;
using Sufka.Game.Persistence;
using Sufka.Game.Popup;
using Sufka.Game.Statistics;
using Sufka.Game.Tutorial;
using Sufka.Game.Unlocks;
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

        private TutorialController _tutorialInstance;

        private readonly StatisticsController _statistics = new StatisticsController();
        private readonly AdsController _ads = new AdsController();
        private GameInProgressSaveData _gameInProgressSaveData;

        private SaveData _saveData;
        private int _pointsForAd;
        private bool _gameInProgress;

        public ColorScheme SelectedColorScheme => _colorSchemeDatabase.ColorSchemes[_saveData.selectedColorSchemeIdx];
        public List<bool> UnlockedGameModes => _saveData.unlockedGameModes;
        public List<bool> UnlockedColors => _saveData.unlockedColors;
        public int PointsSpent => _saveData.pointsSpentOnUnlocks;
        public int PointsSpentOnColors => _saveData.pointsSpentOnColors;
        public int ColorsUnlocked => _saveData.unlockedColorCount;
        public int Score => _saveData.score;
        public int AvailableHints => _saveData.availableHints;
        public bool HintUsed => _gameInProgressSaveData.hintUsed;

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
            SaveGame();
        }

        private void Start()
        {
            LoadGame();

            _playArea.OnHintUsed += HandleHintUsed;
            _playArea.OnWordGuessed += HandleWordGuessed;
            _playArea.OnPointsAwarded += IncreaseScore;
            _playArea.OnHintAdRequested += ShowHintPopup;
            _playArea.OnBackToMenuPopupRequested += ShowBackToMenuPopup;
            _playArea.OnGameProgressUpdated += SaveGameProgress;
            _playArea.OnRoundOver += HandleRoundOver;
            _playArea.OnLetterStatisticsUpdated += HandleLetterStatisticsUpdated;
            _playArea.OnRoundStarted += UpdateGameInProgress;

            _mainMenu.OnRequestGameStart += StartGame;
            _mainMenu.OnRequestContinueGame += ContinueGame;
            _mainMenu.OnRequestUnlockColorScheme += ShowUnlockColorSchemePopup;
            _mainMenu.OnNotifyColorSchemeChanged += UpdateSelectedColorScheme;
            _mainMenu.OnRequestUnlockGameMode += ShowUnlockGameModePopup;
            _mainMenu.Initialize();

            ShowMainMenu();

            _ads.Initialize(this);

            _popup.Initialize();

            if (!_saveData.tutorialCompleted)
            {
                StartTutorial();
            }
        }

        private void UpdateGameInProgress()
        {
            _gameInProgress = true;
            SaveGameProgress();
        }

        private void HandleLetterStatisticsUpdated(int correctLetters, int correctSpotLetters, int typedLetters, int removedLetters)
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

            if (_saveData.wordsUntilHintReward <= 0)
            {
                _saveData.availableHints += HINTS_PER_HINT_REWARD;
                _saveData.ResetWordsUntilHintReward();
                _playArea.RefreshHints();
            }

            SaveGame();
            SaveGameProgress();
        }

        private void ShowUnlockGameModePopup(GameMode gameMode)
        {
            _popup.UnlockGameModePopup(gameMode.Name, gameMode.UnlockCost, Score, () => UnlockGameMode(gameMode));
        }

        private void UnlockGameMode(GameMode gameMode)
        {
            _saveData.score -= gameMode.UnlockCost;
            var gameModeIdx = GetGameModeIdx(gameMode);
            _saveData.unlockedGameModes[gameModeIdx] = true;
            _saveData.pointsSpentOnUnlocks += gameMode.UnlockCost;
            SaveGame();

            _mainMenu.RefreshGameModes();
        }

        private void UpdateSelectedColorScheme(ColorScheme colorScheme)
        {
            var colorSchemeIdx = _colorSchemeDatabase.ColorSchemes.IndexOf(colorScheme);
            _saveData.selectedColorSchemeIdx = colorSchemeIdx;
            SaveGame();
        }

        private void ShowUnlockColorSchemePopup(ColorScheme colorScheme)
        {
            _popup.UnlockColorSchemePopup(colorScheme.Name, colorScheme.UnlockCost, Score,
                                          () => UnlockColorScheme(colorScheme));
        }

        private void UnlockColorScheme(ColorScheme colorScheme)
        {
            _saveData.score -= colorScheme.UnlockCost;

            var colorSchemeIdx = _colorSchemeDatabase.ColorSchemes.IndexOf(colorScheme);
            _saveData.unlockedColors[colorSchemeIdx] = true;

            _saveData.pointsSpentOnColors += colorScheme.UnlockCost;
            _saveData.pointsSpentOnUnlocks += colorScheme.UnlockCost;
            _saveData.unlockedColorCount++;
            
            SaveGame();

            _mainMenu.RefreshColorSchemes();
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
            _popup.ShowHintPopup(HINTS_PER_AD, _ads.PlayHintAd);
        }

        private void IncreaseScore(int points)
        {
            _saveData.score += points;
            _saveData.bonusPointsReward += points;

            _statistics.HandlePointsGained(points, _playArea.GameMode, _availableGameModes);
            
            SaveGame();
        }

        private void HandleWordGuessed(int attempt)
        {
            HandleRoundOver();

            _statistics.HandleWordGuessed(_playArea.GameMode, attempt, _availableGameModes);

            _saveData.wordsUntilBonusPointsReward--;

            if (_saveData.wordsUntilBonusPointsReward <= 0)
            {
                _pointsForAd = _saveData.bonusPointsReward;
                _saveData.ResetWordsUntilBonusPointsReward();

                _popup.BonusPointsPopup(_saveData.wordsUntilBonusPointsReward, _pointsForAd, ClearPointsForAd,
                                        _ads.PlayBonusPointsAd);
            }

            SaveGame();
        }

        private void ClearPointsForAd()
        {
            _pointsForAd = 0;
        }

        private void HandleHintUsed()
        {
            _saveData.availableHints--;
            _statistics.HandleHintUsed(_playArea.GameMode, _availableGameModes);
            SaveGame();
        }

        private void StartGame(GameMode gameMode)
        {
            _playArea.StartGame(gameMode);
        }

        private void ContinueGame()
        {
            var gameMode = _availableGameModes.GameModes[_gameInProgressSaveData.gameModeIdx];
            _playArea.StartGame(gameMode, _gameInProgressSaveData);
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

            if (_saveData.unlockedColors.Count < _colorSchemeDatabase.ColorSchemeCount)
            {
                _saveData.UpdateColorUnlocksCount(_colorSchemeDatabase.ColorSchemeCount);
                SaveGame();
            }

            if (_saveData.unlockedGameModes.Count < _availableGameModes.GameModesCount)
            {
                _saveData.UpdateGameModeUnlocksCount(_availableGameModes.GameModesCount);
                SaveGame();
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
                }
                else if (placementId == _ads.BonusPointsAdId)
                {
                    _saveData.score += _pointsForAd;
                    _playArea.RefreshPoints(_pointsForAd);
                    _statistics.HandlePointsGained(_pointsForAd, _playArea.GameMode, _availableGameModes);
                    ClearPointsForAd();
                    SaveGame();
                }
            }
        }

        public int GetGameModeIdx(GameMode gameMode)
        {
            return _availableGameModes.GameModes.IndexOf(gameMode);
        }
    }
}
