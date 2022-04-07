using System.Collections;
using Sirenix.OdinInspector;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _score;

        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private float _durationPerPoint = .1f;

        [SerializeField]
        private GameController _gameController;

        private int Score => _gameController != null ? _gameController.Score : 0;
        
        public void Initialize()
        {
            if(_playArea!=null)
            {
                _playArea.OnPointsAwarded += RefreshInternal;
                _playArea.OnPointsUpdated += Refresh;
            }

            Refresh();
        }

        private void RefreshInternal(int pointsAwarded)
        {
            var startingPoints = Score - pointsAwarded;
            
            if(gameObject.activeInHierarchy)
            {
                StartCoroutine(CountUp(startingPoints, pointsAwarded));
            }
            else
            {
                DisplayScore(Score);
            }
        }

        public void Refresh()
        {
            DisplayScore(Score);
        }

        private void DisplayScore(int score)
        {
            _score.SetText(NumberFormatting.Format(score));
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
