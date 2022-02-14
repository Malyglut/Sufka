using System;
using System.Collections.Generic;
using Sufka.Game.Validation;
using UnityEngine;

namespace Sufka.Game.Controls
{
    public class KeyGrid : MonoBehaviour
    {
        public event Action<char> OnKeyPress;

        [SerializeField]
        private KeyRow _keyRowPrefab;

        [SerializeField]
        private Transform _parent;

        private readonly List<KeyRow> _rows = new List<KeyRow>();

        private void AddRow()
        {
            var row = Instantiate(_keyRowPrefab, _parent);
            row.OnKeyPress += HandleKeyPress;
            _rows.Add(row);
        }

        private void HandleKeyPress(char letter)
        {
            OnKeyPress.Invoke(letter);
        }

        public void Initialize(List<CharList> charLists)
        {
            for (var row = 0; row < charLists.Count; row++)
            {
                AddRow();

                for (var key = 0; key < charLists[0].chars.Count; key++)
                {
                    var character = charLists[row][key];

                    if (character == '\0')
                    {
                        continue;
                    }

                    _rows[row].AddKey(character);
                }
            }
        }

        public void Refresh(ValidationResult result)
        {
            foreach (var letterResult in result)
            {
                MarkResult(letterResult.letter, letterResult.result);
            }
        }

        private void MarkResult(char letter, LetterCorrectness markType)
        {
            foreach (var row in _rows)
            {
                if (row.Contains(letter))
                {
                    row[letter].Display(markType);
                    break;
                }
            }
        }

        public void Reset()
        {
            foreach (var row in _rows)
            {
                row.Reset();
            }
        }

        public void MarkGuessed(char hintLetter)
        {
            MarkResult(hintLetter, LetterCorrectness.Full);
        }
    }
}
