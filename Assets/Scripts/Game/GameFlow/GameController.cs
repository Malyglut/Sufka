using System;
using Sirenix.OdinInspector;
using Sufka.Ads;
using Sufka.MainMenu;
using Sufka.Persistence;
using Sufka.Popup;
using Sufka.Statistics;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour, IUnityAdsListener
    {
        private const int MAX_AVAILABLE_HINTS = 10;

        public event Action OnAvailableHintsUpdated;

        [SerializeField]
        private MainMenuController _mainMenu;

        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private PopupController _popup;

        private readonly StatisticsController _statistics = new StatisticsController();
        private readonly AdsController _ads = new AdsController();

        public int Score { get; private set; }
        public int AvailableHints { get; private set; } = MAX_AVAILABLE_HINTS;

        private void Start()
        {
            LoadGame();

            _playArea.OnHintUsed += HandleHintUsed;
            _playArea.OnWordGuessed += HandleWordGuessed;
            _playArea.OnPointsAwarded += IncreaseScore;
            _playArea.OnHintAdRequested += ShowHintPopup;

            _mainMenu.OnRequestGameStart += StartGame;

            _ads.Initialize(this);

            _popup.Initialize();
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

        private void SaveGame()
        {
            SaveSystem.SaveGame(Score, AvailableHints, _statistics.WordStatistics);
        }

        private void LoadGame()
        {
            SaveData saveData;

            if (SaveSystem.SaveFileExists())
            {
                saveData = SaveSystem.LoadGame();
            }
            else
            {
                saveData = new SaveData();
            }

            Score = saveData.score;
            AvailableHints = saveData.availableHints;
            AvailableHints = Mathf.Min(AvailableHints, MAX_AVAILABLE_HINTS);

            _statistics.Load(saveData);
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
            // throw new NotImplementedException();
        }

        public void OnUnityAdsDidError(string message)
        {
            // throw new NotImplementedException();
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
               if(placementId == _ads.HintsAdId)
               {
                   AvailableHints = MAX_AVAILABLE_HINTS;
                   SaveGame();

                   OnAvailableHintsUpdated.Invoke();
               }
            }
        }
    }
}
