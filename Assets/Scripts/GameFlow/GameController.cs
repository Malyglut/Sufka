using Sirenix.OdinInspector;
using Sufka.Ads;
using Sufka.MainMenu;
using Sufka.Persistence;
using Sufka.Statistics;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour, IUnityAdsShowListener
    {
        [SerializeField]
        private MainMenuController _mainMenu;

        [SerializeField]
        private PlayAreaController _playArea;

        private StatisticsController _statistics = new StatisticsController();

        private void Start()
        {
            LoadGame();

            _playArea.OnHintUsed += HandleHintUsed;
            _playArea.OnWordGuessed += HandleWordGuessed;
            
            _mainMenu.OnRequestGameStart += StartGame;

            AdsController.Initialize();
        }

        private void HandleWordGuessed(int attempt)
        {
            _statistics.HandleWordGuessed(_playArea.WordLength, attempt);
            SaveGame();
        }

        private void HandleHintUsed()
        {
            _statistics.HandleHintUsed(_playArea.WordLength);
            SaveGame();
        }

        private void StartGame(WordLength wordLength)
        {
            _playArea.StartGame(wordLength);
        }
        
        private void SaveGame()
        {
            SaveSystem.SaveGame(_playArea.Points, _playArea.AvailableHints, _statistics.WordStatistics);
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

            _playArea.Load(saveData);
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

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"AD STARTED {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"AD CLICKED {placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log($"AD COMPLETED {placementId}");
            }
            else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                Debug.Log($"AD SKIPPED {placementId}");
            }
        }
    }
}
