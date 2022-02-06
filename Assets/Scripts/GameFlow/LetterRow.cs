using System.Collections.Generic;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class LetterRow : MonoBehaviour
    {
        [SerializeField]
        private Letter _letterPrefab;

        [SerializeField]
        private Transform _lettersParent;

        private List<Letter> _letters = new List<Letter>();
        private int _currentLetterIdx;
        private int _capacity;
        private int _hintIdx =-1;
        public bool IsFull => _currentLetterIdx == _capacity ||
                              (_currentLetterIdx == _capacity - 1 && _hintIdx == _capacity);
        public bool IsEmpty => _currentLetterIdx == 0;

        public void Initialize(int capacity, Word targetWord)
        {
            _capacity = capacity;
            
            for (int i = 0; i < capacity; i++)
            {
                Letter letter = Instantiate(_letterPrefab, _lettersParent);
                letter.SetBlank();
                
                _letters.Add(letter);
            }

            for (int i = 0; i < targetWord.nonInteractiveLength; i++)
            {
                Letter letter = Instantiate(_letterPrefab, _lettersParent);
                letter.SetLetter(targetWord.nonInteractivePart[i]);
            }
        }

        public void InputLetter(char letter)
        {
            _letters[_currentLetterIdx].SetLetter(letter);

            do
            {
                _currentLetterIdx++;
            }
            while (_currentLetterIdx < _capacity && _currentLetterIdx == _hintIdx);
        }

        public void RemoveLastLetter()
        {
            if (_currentLetterIdx == 0 || _currentLetterIdx == 1 && _hintIdx == 0)
            {
                return;
            }
            
            _currentLetterIdx--;

            if (_currentLetterIdx == _hintIdx)
            {
                _currentLetterIdx--;
            }
            
            _letters[_currentLetterIdx].SetBlank();
        }

        public char[] Word
        {
            get
            {
                char[] word = new char[_letters.Count];

                for (int i = 0; i < word.Length; i++)
                {
                    word[i] = _letters[i].CurrentLetter;
                }

                return word;
            }
        }

        public void Display(ValidationResult result)
        {
            for (int i = 0; i < +_letters.Count; i++)
            {
                _letters[i].Display(result[i].result);
            }
        }

        public void Reset()
        {
            _currentLetterIdx = 0;
            
            foreach (var letter in _letters)
            {
                letter.Reset(true);
            }
        }

        public void MarkGuessed(int hintIdx, char hintLetter)
        {
            _letters[hintIdx].MarkGuessed(hintLetter);

            if (hintIdx == _currentLetterIdx)
            {
                _currentLetterIdx++;
            }

            _hintIdx = hintIdx;
        }

        public void Prepare()
        {
            _currentLetterIdx = 0;

            while (!_letters[_currentLetterIdx].IsBlank)
            {
                _currentLetterIdx++;
            }
        }
    }
}
