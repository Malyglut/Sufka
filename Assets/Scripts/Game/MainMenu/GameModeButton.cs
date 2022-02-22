using System;
using Sufka.Game.GameFlow;
using Sufka.Game.Unlocks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sufka.Game.MainMenu
{
    public class GameModeButton : MonoBehaviour, IPointerDownHandler
    {
        public event Action<GameMode> OnGameModeSelected;
        public event Action<GameMode> OnUnlockRequested;

        [SerializeField]
        private GameMode _gameMode;

        [SerializeField]
        private WordLengthUnlockOverlay _unlockOverlay;

        public GameMode GameMode => _gameMode;

        public void Initialize()
        {
            _unlockOverlay.Initialize();
            _unlockOverlay.OnUnlockRequested += RequestUnlockWordLength;
        }

        private void RequestUnlockWordLength()
        {
            OnUnlockRequested.Invoke(_gameMode);
        }

        public void RefreshAvailability(bool unlocked)
        {
            if (unlocked)
            {
                _unlockOverlay.Disable();
            }
            else
            {
                _unlockOverlay.Enable();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnGameModeSelected.Invoke(_gameMode);
        }
    }
}
