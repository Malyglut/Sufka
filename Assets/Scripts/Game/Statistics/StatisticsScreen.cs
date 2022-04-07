using System.Collections.Generic;
using System.Linq;
using Sufka.Game.GameFlow;
using Sufka.Game.Statistics.Graphs;
using Sufka.Game.Utility;
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
        private SwipeController _swipeController;

        [SerializeField]
        private List<GameObject> _pages = new List<GameObject>();

        [SerializeField]
        private TextMeshProUGUI _statisticsLabel;

        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private StatisticDisplay _guessedWords;

        [SerializeField]
        private BarGraph _attemptsGraph;

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

            _swipeController.OnSwipeRight += ShowPreviousPage;
            _swipeController.OnSwipeLeft += ShowNextPage;
        }

        private void ShowNextPage()
        {
            if (_currentPageIdx < _currentLastPageIdx)
            {
                ShowPage(_currentPageIdx + 1);
            }
        }

        private void ShowPreviousPage()
        {
            if (_currentPageIdx > 0)
            {
                ShowPage(_currentPageIdx - 1);
            }
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

            var overallStatistics = _gameController.GetOverallStatistics();

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

            RefreshAttemptsGraph(statistics);

            _typedLetters.Refresh(statistics.typedLetters);
            _correctLetters.Refresh(statistics.correctLetters);
            _lettersInCorrectSpot.Refresh(statistics.lettersInCorrectSpot);
            _removedLetters.Refresh(statistics.removedLetters);
        }

        private void RefreshAttemptsGraph(WordStatistics statistics)
        {
            var attemptValues = new BarGraphValue[5];

            attemptValues[0] = new BarGraphValue {label = "1", value = statistics.firstAttemptGuesses};
            attemptValues[1] = new BarGraphValue {label = "2", value = statistics.secondAttemptGuesses};
            attemptValues[2] = new BarGraphValue {label = "3", value = statistics.thirdAttemptGuesses};
            attemptValues[3] = new BarGraphValue {label = "4", value = statistics.fourthAttemptGuesses};
            attemptValues[4] = new BarGraphValue {label = "5", value = statistics.fifthAttemptGuesses};

            _attemptsGraph.Initialize(attemptValues);
        }
    }
}
