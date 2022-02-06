using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Controls;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour
    {
        public event Action OnRoundOver;
        public event Action<int> OnPointsAwarded;

        [SerializeField]
        private int _attemptCount = 5;

        [SerializeField]
        private int _letterCount = 5;

        [SerializeField]
        private PlayArea _playArea;

        [SerializeField]
        private Keyboard _keyboard;

        [SerializeField]
        private WordAtlas _wordAtlas;

        private Word _targetWord;

        public int Points { get; private set; }
        public string TargetWordString => _targetWord.fullWord;

        [FoldoutGroup("Debug"), SerializeField, ReadOnly]
        private string _currentWord;

        private List<int> _guessedIndices = new List<int>();
        private bool _hintUsed;

        private void Start()
        {
            _playArea.OnCurrentRowFull += EnableEnter;
            _playArea.OnCurrentRowNotFull += DisableEnter;

            _keyboard.Initialize();
            _keyboard.OnKeyPress += HandleLetterInput;
            _keyboard.OnEnterPress += CheckWord;
            _keyboard.OnBackPress += HandleRemoveLetter;
            _keyboard.OnHintPress += GetHint;

            RandomWordRound();
        }

        private void EnableEnter()
        {
             _keyboard.EnableEnterButton();
        }

        private void DisableEnter()
        {
            _keyboard.DisableEnterButton();
        }

        private void RandomWordRound()
        {
            _targetWord = _wordAtlas.RandomWord();
            StartNewRound();
        }

        private void StartNewRound()
        {
            _hintUsed = false;
            _guessedIndices.Clear();
            
            _currentWord = _targetWord.fullWord;
            
            _playArea.Initialize(_attemptCount, _letterCount, _targetWord);
            _keyboard.Reset();
        }

        private void CheckWord()
        {
            if (_playArea.ValidInput)
            {
                var result = WordValidator.Validate(_targetWord.interactivePart, _playArea.CurrentWord);

                Debug.Log(result);

                foreach (var idx in result.GuessedIndices)
                {
                    if (!_guessedIndices.Contains(idx))
                    {
                        _guessedIndices.Add(idx);
                    }
                }

                if (result.FullMatch)
                {
                    Debug.Log("WIN!");

                    var pointsToAward = _attemptCount - _playArea.Attempt;
                    Points += pointsToAward;
                    OnPointsAwarded.Invoke(pointsToAward);

                    RandomWordRound();
                }
                else if (!result.FullMatch && _playArea.LastAttempt)
                {
                    Debug.Log("LOSE!");
                    OnRoundOver.Invoke();
                    RandomWordRound();
                }
                else
                {
                    _playArea.Display(result);
                    _keyboard.Refresh(result);
                }
            }
        }

        private void HandleRemoveLetter()
        {
            _playArea.RemoveLastLetter();
        }

        private void HandleLetterInput(char letter)
        {
            _playArea.InputLetter(letter);
        }

        [FoldoutGroup("Debug"), Button]
        private void NounRound()
        {
            _targetWord = _wordAtlas.RandomNoun();
            StartNewRound();
        }
        
        [FoldoutGroup("Debug"), Button]
        private void AdjectiveRound()
        {
            _targetWord = _wordAtlas.RandomAdjective();
            StartNewRound();
        }
        
        [FoldoutGroup("Debug"), Button]
        private void VerbRound()
        {
            _targetWord = _wordAtlas.RandomVerb();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void GetHint()
        {
            if (_hintUsed)
            {
                return;
            }
            
            var possibleHints = new List<int>();

            for (int i = 0; i < _targetWord.interactivePart.Length; i++)
            {
                if (_guessedIndices.Contains(i))
                {
                    continue;
                }
                
                possibleHints.Add(i);
            }

            var hintIdx = possibleHints[Random.Range(0, possibleHints.Count)];
            var hintLetter = _targetWord.interactivePart[hintIdx];

            Debug.Log($"HINTING {hintLetter} AT INDEX {hintIdx}");
            
            _guessedIndices.Add(hintIdx);

            _keyboard.MarkGuessed(hintLetter);
            _playArea.MarkGuessed(hintIdx, hintLetter);

            _hintUsed = true;
        }
    }
}
