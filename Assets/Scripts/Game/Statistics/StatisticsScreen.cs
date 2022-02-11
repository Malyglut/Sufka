using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Statistics
{
    public class StatisticsScreen : MonoBehaviour
    {
        [SerializeField]
        private GameController _gameController;
        
        [SerializeField]
        private StatisticDisplay _guessedWords;
        
        [SerializeField]
        private StatisticDisplay _firstAttempt;
        
        [SerializeField]
        private StatisticDisplay _secondAttempt;
        
        [SerializeField]
        private StatisticDisplay _hintsUsed;

        public void Refresh(WordLength wordLength)
        {
            var statistics = _gameController.GetStatistics(wordLength);
            
            _guessedWords.Refresh(statistics.guessedWords);
            _firstAttempt.Refresh(statistics.firstAttemptGuesses);
            _secondAttempt.Refresh(statistics.secondAttemptGuesses);
            _hintsUsed.Refresh(statistics.hintsUsed);
        }
    }
}
