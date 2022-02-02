using System.Collections.Generic;
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

        public void InputLetter(string letter)
        {
            _letters[_currentLetterIdx].SetLetter(letter);
            _currentLetterIdx++;
        }

        public void RemoveLastLetter()
        {
            _letters[_currentLetterIdx-1].SetBlank();
            _currentLetterIdx--;
        }

        public string[] Word
        {
            get
            {
                string[] word = new string[_letters.Count];

                for (int i = 0; i < word.Length; i++)
                {
                    word[i] = _letters[i].CurrentLetter;
                }

                return word;
            }
        }
    }
}
