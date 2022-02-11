using Sirenix.OdinInspector;
using Sufka.MainMenu;
using Sufka.Persistence;
using Sufka.Statistics;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour
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
    }
}
