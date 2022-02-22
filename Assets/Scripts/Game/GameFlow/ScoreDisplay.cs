using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sufka.Game.GameFlow
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _score;

        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private float _durationPerPoint = .1f;

        public void Initialize()
        {
            _playArea.OnPointsAwarded += RefreshInternal;
            _playArea.OnPointsUpdated += Refresh;

            Refresh();
        }

        private void RefreshInternal(int pointsAwarded)
        {
            var startingPoints = _gameController.Score - pointsAwarded;
            StartCoroutine(CountUp(startingPoints, pointsAwarded));
        }

        public void Refresh()
        {
            DisplayScore(_gameController.Score);
        }

        private void DisplayScore(int score)
        {
            _score.SetText(score.ToString());
        }

        private IEnumerator CountUp(int startingAmount, int pointsToAdd)
        {
            var startTime = Time.time;
            var pointsAdded = 1;

            DisplayScore(startingAmount + pointsAdded);

            while (pointsAdded < pointsToAdd)
            {
                var currentTime = Time.time;

                if (currentTime > startTime + _durationPerPoint * pointsAdded)
                {
                    pointsAdded++;
                    DisplayScore(startingAmount + pointsAdded);
                }

                yield return null;
            }
        }

        #if UNITY_EDITOR
        [Button]
        #endif
        private void AddPoints(int points)
        {
            RefreshInternal(points);
        }

        public void Refresh(int pointsToAward)
        {
            RefreshInternal(pointsToAward);
        }
    }
}
