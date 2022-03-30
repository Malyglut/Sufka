using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.GameFlow;
using Sufka.Game.Persistence;
using Sufka.Game.Unlocks;
using Sufka.Game.Validation;
using Sufka.Game.Words;
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
            _tutorialData = GenerateTutorialData();
            _playArea.StartGame(_gameModes.GameModes[_tutorialData.gameModeIdx], _tutorialData);

            foreach (var step in _steps)
            {
                step.OnComplete += ShowNextStep;
            }
            
            ShowCurrentStep();
        }

        private GameInProgressSaveData GenerateTutorialData()
        {
            var tutorialData = new GameInProgressSaveData
                               {
                                   targetWord = new Word(
                                                         WordType.Noun,
                                                         "stres",
                                                         string.Empty
                                                        ),
                                   hintUsed = false,
                                   gameModeIdx = 0,
                               };
            
            return tutorialData;
        }

        [Button]
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
