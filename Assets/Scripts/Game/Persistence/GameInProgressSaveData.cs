using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using Sufka.Game.Validation;
using Sufka.Game.Words;

namespace Sufka.Game.Persistence
{
    [Serializable]
    public class GameInProgressSaveData
    {
        public bool gameInProgress;
        public bool hintUsed;
        public Word targetWord;
        public int gameModeIdx;
        public List<List<LetterResult>> filledLetters = new List<List<LetterResult>>();
        public int hintRow = -1;
        public int hintIdx = -1;
        public List<int> guessedIndices = new List<int>();

        public void UpdateLetters(List<List<LetterResult>> filledLetters)
        {
            this.filledLetters = new List<List<LetterResult>>();
            this.filledLetters.Clear();

            foreach (var letters in filledLetters)
            {
                var rowLetters = new List<LetterResult>(letters);
                this.filledLetters.Add(rowLetters);
            }
        }

        public void Update(bool gameInProgress, int gameModeIdx, PlayAreaController playArea)
        {
            this.gameInProgress = gameInProgress;

            targetWord = playArea.TargetWord;
            hintUsed = playArea.HintUsed;
            this.gameModeIdx = gameModeIdx;
            UpdateLetters(playArea.FilledLetters);
            hintIdx = playArea.HintIdx;
            hintRow = playArea.HintRow;
            guessedIndices = new List<int>(playArea.GuessedIndices);
        }
    }
}
