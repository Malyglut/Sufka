using System.Collections.Generic;
using Sufka.Game.GameFlow;
using Sufka.Game.Persistence;
using Sufka.Game.Unlocks;
using Sufka.Game.Validation;
using UnityEngine;

namespace Sufka.Game.Tutorial
{
    public class TutorialInputAction : MonoBehaviour
    {
        [SerializeField]
        private List<LetterResult> _letters;

        [SerializeField]
        private PlayAreaController _playArea;
        
        [SerializeField]
        private AvailableGameModes _gameModes;

        public void Perform(GameInProgressSaveData tutorialData)
        {
            tutorialData.filledLetters.Add(_letters);
            _playArea.StartGame(_gameModes.GameModes[tutorialData.gameModeIdx], tutorialData);
        }
    }
}
