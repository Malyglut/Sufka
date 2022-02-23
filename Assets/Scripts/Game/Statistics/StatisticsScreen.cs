using System.Collections.Generic;
using System.Linq;
using Sufka.Game.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Statistics
{
    public class StatisticsScreen : MonoBehaviour
    {
        private const string OVERALL_STATISTICS_LABEL = "OGÓLNE";
        
        [SerializeField]
        private List<GameModeStatisticsController> _gameModeStatisticsControllers =
            new List<GameModeStatisticsController>();
        
        [SerializeField]
        private List<GameObject> _pages = new List<GameObject>();
        
        [SerializeField]
        private TextMeshProUGUI _statisticsLabel;
        
        [SerializeField]
        private GameController _gameController;
        
        [SerializeField]
        private StatisticDisplay _guessedWords;
        
        [SerializeField]
        private StatisticDisplay _firstAttempt;
        
        [SerializeField]
        private StatisticDisplay _secondAttempt;
        
        [SerializeField]
        private StatisticDisplay _thirdAttempt;
        
        [SerializeField]
        private StatisticDisplay _fourthAttempt;
        
        [SerializeField]
        private StatisticDisplay _fifthAttempt;
        
        [SerializeField]
        private StatisticDisplay _hintsUsed;
        
        [SerializeField]
        private StatisticDisplay _pointsGained;
        
        [SerializeField]
        private StatisticDisplay _pointsSpent;
        
        [SerializeField]
        private StatisticDisplay _pointsSpentOnColors;
        
        [SerializeField]
        private StatisticDisplay _colorsUnlocked;
        
        [SerializeField]
        private StatisticDisplay _typedLetters;
        
        [SerializeField]
        private StatisticDisplay _correctLetters;
        
        [SerializeField]
        private StatisticDisplay _lettersInCorrectSpot;
        
        [SerializeField]
        private StatisticDisplay _removedLetters;

        [SerializeField]
        private Button _previousPageButton;

        [SerializeField]
        private Button _nextPageButton;

        private int _currentPageIdx;
        private int _currentLastPageIdx;
        private GameModeStatisticsController _previousSelected;

        public void Initialize()
        {
            _previousPageButton.onClick.AddListener(ShowPreviousPage);
            _nextPageButton.onClick.AddListener(ShowNextPage);
        }

        private void ShowNextPage()
        {
            ShowPage(_currentPageIdx + 1);
        }

        private void ShowPreviousPage()
        {
            ShowPage(_currentPageIdx - 1);
        }

        public void Refresh(GameMode gameMode)
        {
            var statistics = _gameController.GetStatistics(gameMode);
            RefreshStatistics(gameMode.Name, statistics);

            if (_previousSelected != null)
            {
                _previousSelected.Revert();
            }
            
            var statisticsController =
                _gameModeStatisticsControllers.First(controller => controller.GameMode == gameMode);

            statisticsController.Prepare();
            _currentLastPageIdx = statisticsController.LastPageIdx;

            _previousSelected = statisticsController;
            
            ShowPage(0);
        }

        private void ShowPage(int pageIdx)
        {
            _pages[_currentPageIdx].SetActive(false);

            _currentPageIdx = pageIdx;
            _pages[_currentPageIdx].SetActive(true);
            
            _previousPageButton.gameObject.SetActive(_currentPageIdx > 0);
            _nextPageButton.gameObject.SetActive(_currentPageIdx < _currentLastPageIdx);
        }

        public void RefreshOverall()
        {
            if (_previousSelected != null)
            {
                _previousSelected.Revert();
            }
            
            var overallStatistics = new WordStatistics(-1);

            foreach (var controller in _gameModeStatisticsControllers)
            {
                var gameModeStatistics = _gameController.GetStatistics(controller.GameMode);

                overallStatistics.guessedWords += gameModeStatistics.guessedWords;
                overallStatistics.hintsUsed += gameModeStatistics.hintsUsed;
                overallStatistics.scoreGained += gameModeStatistics.scoreGained;

                overallStatistics.firstAttemptGuesses += gameModeStatistics.firstAttemptGuesses;
                overallStatistics.secondAttemptGuesses += gameModeStatistics.secondAttemptGuesses;
                overallStatistics.thirdAttemptGuesses += gameModeStatistics.thirdAttemptGuesses;
                overallStatistics.fourthAttemptGuesses += gameModeStatistics.fourthAttemptGuesses;
                overallStatistics.fifthAttemptGuesses += gameModeStatistics.fifthAttemptGuesses;
                
                overallStatistics.typedLetters += gameModeStatistics.typedLetters;
                overallStatistics.correctLetters += gameModeStatistics.correctLetters;
                overallStatistics.lettersInCorrectSpot += gameModeStatistics.lettersInCorrectSpot;
                overallStatistics.removedLetters += gameModeStatistics.removedLetters;
            }
            
            _pointsSpent.Refresh(_gameController.PointsSpent);
            _pointsSpentOnColors.Refresh(_gameController.PointsSpentOnColors);
            _colorsUnlocked.Refresh(_gameController.ColorsUnlocked);

            RefreshStatistics(OVERALL_STATISTICS_LABEL, overallStatistics);
            _currentLastPageIdx = _pages.Count - 1;
            
            ShowPage(0);
        }

        private void RefreshStatistics(string label, WordStatistics statistics)
        {
            _statisticsLabel.SetText(label);

            _guessedWords.Refresh(statistics.guessedWords);
            _hintsUsed.Refresh(statistics.hintsUsed);
            _pointsGained.Refresh(statistics.scoreGained);

            _firstAttempt.Refresh(statistics.firstAttemptGuesses);
            _secondAttempt.Refresh(statistics.secondAttemptGuesses);
            _thirdAttempt.Refresh(statistics.thirdAttemptGuesses);
            _fourthAttempt.Refresh(statistics.fourthAttemptGuesses);
            _fifthAttempt.Refresh(statistics.fifthAttemptGuesses);
            
            _typedLetters.Refresh(statistics.typedLetters);
            _correctLetters.Refresh(statistics.correctLetters);
            _lettersInCorrectSpot.Refresh(statistics.lettersInCorrectSpot);
            _removedLetters.Refresh(statistics.removedLetters);
        }
    }
}
