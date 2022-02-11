using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sufka.Game.GameFlow
{
    public class ScoreDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("_points"), SerializeField]
        private TextMeshProUGUI _score;

        [FormerlySerializedAs("_gameController"), SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private float _durationPerPoint = .1f;

        private void Start()
        {
            _playArea.OnPointsAwarded += Refresh;
            _playArea.OnPointsUpdated += Refresh;

            Refresh();
        }

        private void Refresh(int pointsAwarded)
        {
            var startingPoints = _gameController.Score - pointsAwarded;
            StartCoroutine(CountUp(startingPoints, pointsAwarded));
        }

        private void Refresh()
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

        [Button]
        private void AddPoints(int points)
        {
            Refresh(points);
        }
    }
}
