using System;
using Sufka.Game.Words;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.GameFlow
{
    public class RoundOverDisplay : MonoBehaviour
    {
        public event Action OnNextRoundRequested;
        
        [SerializeField]
        private GameObject _root;

        [SerializeField]
        private RoundWonDisplay _win;
        
        [SerializeField]
        private RoundLostDisplay _loss;

        [SerializeField]
        private Button _continueButton;

        public void Initialize()
        {
            _continueButton.onClick.AddListener(RequestNextRound);
        }

        private void RequestNextRound()
        {
            OnNextRoundRequested.Invoke();
        }

        public void Hide()
        {
            _root.gameObject.SetActive(false);
        }

        public void ShowWin(int pointGained)
        {
            _root.gameObject.SetActive(true);

            _loss.gameObject.SetActive(false);
            _win.gameObject.SetActive(true);

            _win.Refresh(pointGained);
        }

        public void ShowLoss(int letterCount, Word word)
        {
            _root.gameObject.SetActive(true);

            _loss.gameObject.SetActive(true);
            _win.gameObject.SetActive(false);

            _loss.Refresh(letterCount, word);
        }
    }
}
