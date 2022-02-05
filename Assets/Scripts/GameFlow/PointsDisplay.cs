using TMPro;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class PointsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _points;

        [SerializeField]
        private GameController _gameController;

        private void Start()
        {
            _gameController.OnPointsAwarded += Refresh;
        }

        private void Refresh(int pointsAwarded)
        {
            _points.SetText(_gameController.Points.ToString());
        }
    }
}
