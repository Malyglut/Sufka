using System.Collections.Generic;
using Sufka.Validation;
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
        public bool IsFull => _currentLetterIdx == _capacity;
        public bool IsEmpty => _currentLetterIdx == 0;

        public void Initialize(int capacity)
        {
            _capacity = capacity;
            
            for (int i = 0; i < capacity; i++)
            {
                Letter letter = Instantiate(_letterPrefab, _lettersParent);
                letter.SetBlank();
                
                _letters.Add(letter);
            }
        }

        public void InputLetter(char letter)
        {
            _letters[_currentLetterIdx].SetLetter(letter);
            _currentLetterIdx++;
        }

        public void RemoveLastLetter()
        {
            _letters[_currentLetterIdx-1].SetBlank();
            _currentLetterIdx--;
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
    }
}
