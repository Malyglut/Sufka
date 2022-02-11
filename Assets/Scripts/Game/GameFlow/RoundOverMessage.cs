using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class RoundOverMessage : MonoBehaviour
    {
        [SerializeField]
        private GameObject _messageContainer;

        [SerializeField]
        private TextMeshProUGUI _message;

        [SerializeField]
        private TextMeshProUGUI _pointsAwarded;

        [SerializeField]
        private PlayAreaController _gameController;
        
        [SerializeField]
        private Color _winColor = Color.white;
        
        [SerializeField]
        private Color _lossColor = Color.white;

        [SerializeField]
        private float _fadeDuration = 2f;

        [SerializeField]
        private List<string> _winMessages = new List<string>();
        
        private Coroutine _currentFade;

        private void Start()
        {
            _gameController.OnPointsAwarded += DisplayPoints;
            _gameController.OnRoundOver += DisplayLoss;
        }

        private void DisplayPoints(int pointsAwarded)
        {
            DisplayMessage(_winMessages[Random.Range(0, _winMessages.Count)], $"+ {pointsAwarded}", _winColor);
        }

        private void DisplayLoss()
        {
            DisplayMessage(_gameController.TargetWordString, string.Empty, _lossColor);
        }

        private void DisplayMessage(string message, string points, Color color)
        {
            _message.SetText(message);
            _pointsAwarded.SetText(points);

            _message.color = color;
            _pointsAwarded.color = color;

            StartFade();
        }

        private void StartFade()
        {
            if (_currentFade != null)
            {
                StopCoroutine(_currentFade);
            }

            _currentFade = StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            _messageContainer.SetActive(true);
            yield return new WaitForSecondsRealtime(_fadeDuration);
            _messageContainer.SetActive(false);

            _currentFade = null;
        }
    }
}
