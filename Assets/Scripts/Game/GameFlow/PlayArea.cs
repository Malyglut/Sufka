using System;
using System.Collections.Generic;
using Sufka.Game.Validation;
using Sufka.Game.Words;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class PlayArea : MonoBehaviour
    {
        public event Action OnCurrentRowFull;
        public event Action OnCurrentRowNotFull;

        [SerializeField]
        private LetterRow _letterRowPrefab;

        [SerializeField]
        private Transform _rowsParent;

        private readonly List<LetterRow> _rows = new List<LetterRow>();

        public int CurrentAttempt { get; private set; }

        private LetterRow CurrentRow => _rows[Mathf.Min(CurrentAttempt, _rows.Count-1)];
        public char[] CurrentWord => CurrentRow.Word;
        public bool ValidInput => CurrentRow.IsFull;
        public bool LastAttempt => CurrentAttempt >= _rows.Count - 1;
        public int TypedLetters { get; private set; }
        public int RemovedLetters { get; private set; }

        public List<List<LetterResult>> GetFilledLetters()
        {
            var letters = new List<List<LetterResult>>();

            foreach (var row in _rows)
            {
                if (row.IsEmpty)
                {
                    break;
                }

                var rowLetters = new List<LetterResult>();

                rowLetters.AddRange(row.GetLetters());

                letters.Add(rowLetters);
            }

            return letters;
        }

        public void Initialize(int rows, int letters, Word targetWord)
        {
            Reset();

            for (var i = 0; i < rows; i++)
            {
                var row = Instantiate(_letterRowPrefab, _rowsParent);
                row.Initialize(letters, targetWord);

                _rows.Add(row);
            }
        }

        public void RemoveLastLetter()
        {
            if (!CurrentRow.IsEmpty)
            {
                RemovedLetters++;
                CurrentRow.RemoveLastLetter();

                if (!CurrentRow.IsFull)
                {
                    OnCurrentRowNotFull.Invoke();
                }
            }
        }

        public void InputLetter(char letter)
        {
            if (!CurrentRow.IsFull)
            {
                TypedLetters++;
                CurrentRow.InputLetter(letter);

                if (CurrentRow.IsFull)
                {
                    OnCurrentRowFull.Invoke();
                }
            }
        }

        public void Display(ValidationResult result)
        {
            TypedLetters = 0;
            RemovedLetters = 0;
            CurrentRow.Display(result);
            CurrentAttempt++;
            CurrentRow.Prepare();
        }

        public void Reset()
        {
            TypedLetters = 0;
            RemovedLetters = 0;
            CurrentAttempt = 0;

            foreach (var row in _rows)
            {
                Destroy(row.gameObject);
            }

            _rows.Clear();
        }

        public void MarkGuessed(int hintIdx, char hintLetter)
        {
            _rows[CurrentAttempt].MarkHint(hintIdx, hintLetter);
        }

        public void RestoreLetters(List<List<LetterResult>> filledLetters)
        {
            for (var i = 0; i < filledLetters.Count; i++)
            {
                _rows[i].RestoreLetters(filledLetters[i]);
            }

            while (CurrentRow.IsFull)
            {
                CurrentAttempt++;
            }
        }

        public void MarkHint(int hintRow, int hintIdx, char letter)
        {
            _rows[hintRow].MarkHint(hintIdx, letter);
        }
    }
}
