using System;
using Sirenix.OdinInspector;
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
        private MainMenuController _mainMenu;

        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private PopupController _popup;

        private readonly StatisticsController _statistics = new StatisticsController();
        private readonly AdsController _ads = new AdsController();
        public UnlocksData _unlocks { get; private set; } = new UnlocksData();
        private GameInProgressSaveData _gameInProgressSaveData;

        public int Score { get; private set; }
        public int AvailableHints { get; private set; } = INITIAL_AVAILABLE_HINTS;
        public ColorScheme SelectedColorScheme => _colorSchemeDatabase.ColorSchemes[_unlocks.selectedColorSchemeIdx];

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
            _mainMenu.Initialize();

            ShowMainMenu();

            _ads.Initialize(this);

            _popup.Initialize();
        }

        private void UpdateSelectedColorScheme(ColorScheme colorScheme)
        {
            var colorSchemeIdx = _colorSchemeDatabase.ColorSchemes.IndexOf(colorScheme);
            _unlocks.selectedColorSchemeIdx = colorSchemeIdx;
            SaveSystem.SaveUnlocksData(_unlocks);
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
            _unlocks.unlockedColors[colorSchemeIdx] = true;
            SaveSystem.SaveUnlocksData(_unlocks);

            _mainMenu.RefreshColorSchemes();
        }

        private void SaveGameProgress()
        {
            _gameInProgressSaveData.Update(true, _playArea);

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
            _statistics.HandleWordGuessed(_playArea.WordLength, attempt);
            SaveGame();
        }

        private void HandleHintUsed()
        {
            AvailableHints--;
            _statistics.HandleHintUsed(_playArea.WordLength);
            SaveGame();
        }

        private void StartGame(WordLength wordLength)
        {
            _playArea.StartGame(wordLength);
        }

        private void ContinueGame()
        {
            _playArea.StartGame(_gameInProgressSaveData);
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

            _statistics.Load(saveData);

            _gameInProgressSaveData = SaveSystem.GameInProgressFileExists() ?
                                          SaveSystem.LoadGameInProgress() :
                                          new GameInProgressSaveData();

            _unlocks = SaveSystem.UnlocksFileExists() ? SaveSystem.LoadUnlocks() : new UnlocksData();

            if (_unlocks.unlockedColors.Count < _colorSchemeDatabase.ColorSchemeCount)
            {
                _unlocks.UpdateColorUnlocksCount(_colorSchemeDatabase.ColorSchemeCount);
                SaveSystem.SaveUnlocksData(_unlocks);
            }
        }

        [FoldoutGroup("Debug"), Button]
        private void LogStatistics(WordLength wordLength)
        {
            var statistics = _statistics.GetStatistics(wordLength);

            Debug.Log($"GUESSED WORDS: {statistics.guessedWords}");
            Debug.Log($"FIRST ATTEMPT GUESSES: {statistics.firstAttemptGuesses}");
            Debug.Log($"SECOND ATTEMPT GUESSES: {statistics.secondAttemptGuesses}");
            Debug.Log($"HINTS USED: {statistics.hintsUsed}");
        }

        public WordStatistics GetStatistics(WordLength wordLength)
        {
            return _statistics.GetStatistics(wordLength);
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
                    AvailableHints = INITIAL_AVAILABLE_HINTS;
                    SaveGame();

                    OnAvailableHintsUpdated.Invoke();
                }
            }
        }
    }
}
