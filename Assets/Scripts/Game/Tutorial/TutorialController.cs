using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using Sufka.Game.Persistence;
using Sufka.Game.Unlocks;
using UnityEngine;

namespace Sufka.Game.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        public event Action OnComplete;
        
        [SerializeField]
        private PlayAreaController _playArea;

        [SerializeField]
        private AvailableGameModes _gameModes;

        [SerializeField]
        private List<TutorialStep> _steps = new List<TutorialStep>();

        private int _currentStep;
        private GameInProgressSaveData _tutorialData;

        private void Start()
        {
            _tutorialData = SaveSystem.LoadTutorial();
            _playArea.StartGame(_gameModes.GameModes[_tutorialData.gameModeIdx], _tutorialData);

            foreach (var step in _steps)
            {
                step.OnComplete += ShowNextStep;
                step.Initialize();
            }
            
            ShowCurrentStep();
        }

        private void ShowNextStep()
        {
            _steps[_currentStep].Hide();
            _currentStep++;
            ShowCurrentStep();
        }

        private void ShowCurrentStep()
        {
            if (_currentStep < _steps.Count)
            {
                _steps[_currentStep].Show(_tutorialData);
            }
            else
            {
                OnComplete.Invoke();
            }
        }
    }
}
