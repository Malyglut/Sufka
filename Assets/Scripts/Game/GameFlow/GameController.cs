using System;
using Sufka.Game.Ads;
using Sufka.Game.Colors;
using Sufka.Game.MainMenu;
using Sufka.Game.Persistence;
using Sufka.Game.Popup;
using Sufka.Game.Statistics;
using Sufka.Game.Unlocks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Sufka.Game.GameFlow
{
    public class GameController : MonoBehaviour, IUnityAdsListener
    {
        private const int INITIAL_AVAILABLE_HINTS = 10;

        public event Action OnAvailableHintsUpdated;

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

        private readonly StatisticsController _statistics = new StatisticsController();
        private readonly AdsController _ads = new AdsController();
        public UnlocksData Unlocks { get; private set; } = new UnlocksData();
        private GameInProgressSaveData _gameInProgressSaveData;

        public int Score { get; private set; }
        public int AvailableHints { get; private set; } = INITIAL_AVAILABLE_HINTS;
        public ColorScheme SelectedColorScheme => _colorSchemeDatabase.ColorSchemes[Unlocks.selectedColorSchemeIdx];

        private void Start()
        {
            LoadGame();

            _playArea.OnHintUsed += HandleHintUsed;
            _playArea.OnWordGuessed += HandleWordGuessed;
            _playArea.OnPointsAwarded += IncreaseScore;
            _playArea.OnHintAdRequested += ShowHintPopup;
            _playArea.OnBackToMenuPopupRequested += ShowBackToMenuPopup;
            _playArea.OnGameProgressUpdated += SaveGameProgress;

            _mainMenu.OnRequestGameStart += StartGame;
            _mainMenu.OnRequestContinueGame += ContinueGame;
            _mainMenu.OnRequestUnlockColorScheme += ShowUnlockColorSchemePopup;
            _mainMenu.OnNotifyColorSchemeChanged += UpdateSelectedColorScheme;
            _mainMenu.OnRequestUnlockGameMode += ShowUnlockGameModePopup;
            _mainMenu.Initialize();

            ShowMainMenu();

            _ads.Initialize(this);

            _popup.Initialize();
        }

        private void ShowUnlockGameModePopup(GameMode wordLength)
        {
            // _popup.UnlockGameModePopup();
        }

        private void UpdateSelectedColorScheme(ColorScheme colorScheme)
        {
            var colorSchemeIdx = _colorSchemeDatabase.ColorSchemes.IndexOf(colorScheme);
            Unlocks.selectedColorSchemeIdx = colorSchemeIdx;
            SaveSystem.SaveUnlocksData(Unlocks);
        }

        private void ShowUnlockColorSchemePopup(ColorScheme colorScheme)
        {
            _popup.UnlockColorSchemePopup(colorScheme.Name, colorScheme.UnlockCost, Score, ()=> UnlockColorScheme(colorScheme));
        }

        private void UnlockColorScheme(ColorScheme colorScheme)
        {
            Score -= colorScheme.UnlockCost;
            SaveGame();
            
            var colorSchemeIdx = _colorSchemeDatabase.ColorSchemes.IndexOf(colorScheme);
            Unlocks.unlockedColors[colorSchemeIdx] = true;
            SaveSystem.SaveUnlocksData(Unlocks);

            _mainMenu.RefreshColorSchemes();
        }

        private void SaveGameProgress()
        {
            var gameModeIdx = _availableGameModes.GameModes.IndexOf(_playArea.GameMode);
            _gameInProgressSaveData.Update(true, gameModeIdx, _playArea);

            SaveSystem.SaveGameInProgress(_gameInProgressSaveData);
        }

        private void ShowBackToMenuPopup()
        {
            _popup.ShowBackToMenuPopup(ShowMainMenu);
        }

        private void ShowMainMenu()
        {
            _playArea.Hide();
            _mainMenu.ShowTitleScreen(_gameInProgressSaveData.gameInProgress);
        }

        private void ShowHintPopup()
        {
            _popup.ShowHintPopup(PlayHintAd);
        }

        private void PlayHintAd()
        {
            _ads.PlayHintAd();
        }

        private void IncreaseScore(int points)
        {
            Score += points;
        }

        private void HandleWordGuessed(int attempt)
        {
            _statistics.HandleWordGuessed(_playArea.GameMode, attempt, _availableGameModes);
            SaveGame();
        }

        private void HandleHintUsed()
        {
            AvailableHints--;
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
            SaveSystem.SaveGame(Score, AvailableHints, _statistics.WordStatistics);
        }

        private void LoadGame()
        {
            SaveData saveData = SaveSystem.SaveFileExists() ? SaveSystem.LoadGame() : new SaveData();

            Score = saveData.score;
            AvailableHints = saveData.availableHints;
            AvailableHints = Mathf.Min(AvailableHints, INITIAL_AVAILABLE_HINTS);

            _statistics.Load(saveData, _availableGameModes);

            _gameInProgressSaveData = SaveSystem.GameInProgressFileExists() ?
                                          SaveSystem.LoadGameInProgress() :
                                          new GameInProgressSaveData();

            Unlocks = SaveSystem.UnlocksFileExists() ? SaveSystem.LoadUnlocks() : new UnlocksData();

            if (Unlocks.unlockedColors.Count < _colorSchemeDatabase.ColorSchemeCount)
            {
                Unlocks.UpdateColorUnlocksCount(_colorSchemeDatabase.ColorSchemeCount);
                SaveSystem.SaveUnlocksData(Unlocks);
            }

            if (Unlocks.unlockedGameModes.Count < _availableGameModes.GameModesCount)
            {
                Unlocks.UpdateGameModeUnlocksCount(_availableGameModes.GameModesCount);
                SaveSystem.SaveUnlocksData(Unlocks);
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
            Debug.Log($"AD STARTED {placementId}");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Debug.Log($"AD FINISHED {placementId}");

            if (showResult == ShowResult.Finished)
            {
                if (placementId == _ads.HintsAdId)
                {
                    AvailableHints += INITIAL_AVAILABLE_HINTS;
                    SaveGame();

                    OnAvailableHintsUpdated.Invoke();
                }
                else if (placementId == _ads.BonusPointsAdId)
                {
                    
                }
            }
        }

        public int GetGameModeIdx(GameMode gameMode)
        {
            return _availableGameModes.GameModes.IndexOf(gameMode);
        }
    }
}
