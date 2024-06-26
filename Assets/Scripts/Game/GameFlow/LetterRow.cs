using System;
using System.Collections.Generic;
using Sufka.Game.Validation;
using Sufka.Game.Words;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class LetterRow : MonoBehaviour
    {
        [SerializeField]
        private InteractiveLetter _letterPrefab;

        [SerializeField]
        private NonInteractiveLetter _nonInteractiveLetterPrefab;

        [SerializeField]
        private Transform _interactiveLettersParent;

        [SerializeField]
        private Transform _nonInteractiveLettersParent;

        private readonly List<InteractiveLetter> _letters = new List<InteractiveLetter>();
        private int _currentLetterIdx;
        private int _capacity;
        private int _hintIdx = -1;
        public bool IsFull => _currentLetterIdx == _capacity ||
                              _currentLetterIdx == _capacity - 1 && _hintIdx == _capacity;
        public bool IsEmpty => _currentLetterIdx == 0 && _hintIdx == -1;

        public char[] Word
        {
            get
            {
                var word = new char[_letters.Count];

                for (var i = 0; i < word.Length; i++)
                {
                    word[i] = _letters[i].CurrentLetter;
                }

                return word;
            }
        }

        public void Initialize(int capacity, Word targetWord)
        {
            _capacity = capacity;

            for (var i = 0; i < capacity; i++)
            {
                var interactiveLetter = Instantiate(_letterPrefab, _interactiveLettersParent);
                interactiveLetter.SetEmpty();
                interactiveLetter.Initialize();

                _letters.Add(interactiveLetter);
            }

            for (var i = 0; i < targetWord.nonInteractiveLength; i++)
            {
                var letter = Instantiate(_nonInteractiveLetterPrefab, _nonInteractiveLettersParent);
                letter.SetLetter(targetWord.nonInteractivePart[i]);

                letter.Initialize();
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

            _letters[_currentLetterIdx].SetEmpty();
        }

        public void Display(ValidationResult result)
        {
            for (var i = 0; i < +_letters.Count; i++)
            {
                _letters[i].Display(result[i].result);
            }
        }

        public void Reset()
        {
            _currentLetterIdx = 0;

            foreach (var letter in _letters)
            {
                letter.SetEmpty();
            }
        }

        public void MarkHint(int hintIdx, char hintLetter)
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

            while (_currentLetterIdx < _letters.Count && !_letters[_currentLetterIdx].IsBlank)
            {
                _currentLetterIdx++;
            }
        }

        public List<LetterResult> GetLetters()
        {
            var letters = new List<LetterResult>();

            foreach (var letter in _letters)
            {
                letters.Add(letter.CurrentLetterResult);
            }

            return letters;
        }

        public void RestoreLetters(List<LetterResult> filledLetters)
        {
            for (var i = 0; i < filledLetters.Count; i++)
            {
                var letterResult = filledLetters[i];
                _letters[i].SetLetter(letterResult.letter);
                _letters[i].Display(letterResult.result);

                if (i == _currentLetterIdx && !_letters[i].IsBlank)
                {
                    _currentLetterIdx++;
                }
            }
        }

        public void DisplayFailed(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                _letters[i].SetLetter(word[i]);
            }
        }
    }
}
