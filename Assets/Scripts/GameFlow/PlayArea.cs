using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
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

        [SerializeField]
        private string _targetWord;

        private List<LetterRow> _rows = new List<LetterRow>();
        private int _currentRowIdx;
        private List<KeyCode> _allKeyCodes = new List<KeyCode>();

        private LetterRow CurrentRow => _rows[_currentRowIdx];
        
        private void Start()
        {
            _allKeyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToList();

            Initialize(5,5);
        }

        [Button]
        private void Initialize(int rows, int letters)
        {
            for (int i = 0; i < rows; i++)
            {
                LetterRow row = Instantiate(_letterRowPrefab, _rowsParent);
                row.Initialize(letters);

                _rows.Add(row);
            }
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (var keyCode in _allKeyCodes)
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        var input = keyCode.ToString();

                        if (Regex.IsMatch(input, "^[a-zA-Z]{1}$"))
                        {
                            InputLetter(input);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveLastLetter();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                AcceptWord();
            }
        }

        private void AcceptWord()
        {
            if (CurrentRow.IsFull)
            {
                var result = WordValidator.Validate(_targetWord, CurrentRow.Word);

                Debug.Log(result);

                if (result.FullMatch)
                {
                    Debug.Log("WIN!");
                }
                
                _currentRowIdx++;
            }
        }

        private void RemoveLastLetter()
        {
            if(!CurrentRow.IsEmpty)
            {
                CurrentRow.RemoveLastLetter();
            }
        }

        private void InputLetter(string letter)
        {
            if(!CurrentRow.IsFull)
            {
                CurrentRow.InputLetter(letter);
            }
        }
    }
}
