using System.Collections.Generic;
using Sufka.Validation;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class PlayArea : MonoBehaviour
    {
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

        public void Initialize(int rows, int letters)
        {
            for (var i = 0; i < rows; i++)
            {
                var row = Instantiate(_letterRowPrefab, _rowsParent);
                row.Initialize(letters);

                _rows.Add(row);
            }
        }

        public void RemoveLastLetter()
        {
            if (!CurrentRow.IsEmpty)
            {
                CurrentRow.RemoveLastLetter();
            }
        }

        public void InputLetter(char letter)
        {
            if (!CurrentRow.IsFull)
            {
                CurrentRow.InputLetter(letter);
            }
        }

        public void Display(ValidationResult result)
        {
            CurrentRow.Display(result);
            _currentRowIdx++;
        }

        public void Reset()
        {
            _currentRowIdx = 0;

            foreach (var row in _rows)
            {
                row.Reset();
            }
        }
    }
}
