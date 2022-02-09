using System;
using System.Collections.Generic;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;

namespace Sufka.GameFlow
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
        private int _currentRowIdx;

        private LetterRow CurrentRow => _rows[_currentRowIdx];
        public char[] CurrentWord => CurrentRow.Word;
        public bool ValidInput => CurrentRow.IsFull;
        public bool LastAttempt => _currentRowIdx >= _rows.Count - 1;

        public int Attempt => _currentRowIdx;

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
                CurrentRow.RemoveLastLetter();
                
                if(!CurrentRow.IsFull)
                {
                    OnCurrentRowNotFull.Invoke();
                }
            }
        }

        public void InputLetter(char letter)
        {
            if (!CurrentRow.IsFull)
            {
                CurrentRow.InputLetter(letter);

                if (CurrentRow.IsFull)
                {
                    OnCurrentRowFull.Invoke();
                }
            }
        }

        public void Display(ValidationResult result)
        {
            CurrentRow.Display(result);
            _currentRowIdx++;
            CurrentRow.Prepare();
        }

        public void Reset()
        {
            _currentRowIdx = 0;

            foreach (var row in _rows)
            {
                Destroy(row.gameObject);
            }
            
            _rows.Clear();
        }

        public void MarkGuessed(int hintIdx, char hintLetter)
        { 
            _rows[_currentRowIdx].MarkGuessed(hintIdx, hintLetter);
        }
    }
}
