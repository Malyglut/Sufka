using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class PointsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _points;

        [SerializeField]
        private PlayAreaController _gameController;

        [SerializeField]
        private float _durationPerPoint = .1f;

        private void Start()
        {
            _gameController.OnPointsAwarded += Refresh;
            _gameController.OnPointsUpdated += Refresh;
            
            Refresh();
        }

        private void Refresh(int pointsAwarded)
        {

                var startingPoints = _gameController.Points - pointsAwarded;
                StartCoroutine(CountUp(startingPoints, pointsAwarded));
 
        }

        private void Refresh()
        {
            DisplayScore(_gameController.Points);
        }

        private void DisplayScore(int score)
        {
            _points.SetText(score.ToString());
        }

        private IEnumerator CountUp(int startingAmount, int pointsToAdd)
        {
            var startTime = Time.time;
            var pointsAdded = 1;
            
            DisplayScore(startingAmount + pointsAdded);

            while (pointsAdded<pointsToAdd)
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
