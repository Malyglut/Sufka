using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Controls;
using Sufka.Persistence;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour
    {
        public event Action OnRoundOver;
        public event Action OnPointsUpdated;
        public event Action<int> OnPointsAwarded;
        public event Action<Word> OnRoundStarted;

        [SerializeField]
        private PlayArea _playArea;

        [SerializeField]
        private Keyboard _keyboard;

        [SerializeField]
        private WordLength _wordLength;

        [SerializeField]
        private GameSetupDatabase _gameSetupDatabase;
        
        [FoldoutGroup("Debug"), SerializeField, ReadOnly]
        private string _currentWord;

        private readonly List<int> _guessedIndices = new List<int>();

        private Word _targetWord;
        private bool _hintUsed;

        private GameSetup _currentGameSetup;

        public int Points { get; private set; }
        public int AvailableHints { get; private set; } = int.MaxValue;
        public string TargetWordString => _targetWord.fullWord;

        private void Start()
        {
            if (SaveSystem.SaveFileExists())
            {
                var saveData = SaveSystem.LoadGame();
                Points = saveData.score;

                OnPointsUpdated.Invoke();
            }
            else
            {
                Debug.Log("NO SAVE DATA FOUND");
            }

            Initialize(_wordLength);
            RandomWordRound();
        }

        private void Initialize(WordLength wordLength)
        {
            _currentGameSetup = _gameSetupDatabase[wordLength];

            _playArea.OnCurrentRowFull += EnableEnter;
            _playArea.OnCurrentRowNotFull += DisableEnter;

            _keyboard.Initialize();
            _keyboard.OnKeyPress += HandleLetterInput;
            _keyboard.OnEnterPress += CheckWord;
            _keyboard.OnBackPress += HandleRemoveLetter;
            _keyboard.OnHintPress += GetHint;
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
            _targetWord = _currentGameSetup.WordAtlas.RandomWord();
            StartNewRound();
        }

        private void StartNewRound()
        {
            OnRoundStarted.Invoke(_targetWord);

            _hintUsed = false;
            _guessedIndices.Clear();

            _currentWord = _targetWord.fullWord;

            _playArea.Initialize(_currentGameSetup.AttemptCount, _currentGameSetup.LetterCount, _targetWord);
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

                    var pointsToAward = _currentGameSetup.AttemptCount - _playArea.Attempt;
                    Points += pointsToAward;
                    OnPointsAwarded.Invoke(pointsToAward);

                    SaveGame();

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

        private void SaveGame()
        {
            SaveSystem.SaveGame(Points, AvailableHints);
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
            _targetWord = _currentGameSetup.WordAtlas.RandomNoun();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void AdjectiveRound()
        {
            _targetWord = _currentGameSetup.WordAtlas.RandomAdjective();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void VerbRound()
        {
            _targetWord = _currentGameSetup.WordAtlas.RandomVerb();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void GetHint()
        {
            if (_hintUsed || AvailableHints == 0)
            {
                return;
            }

            var possibleHints = new List<int>();

            for (var i = 0; i < _targetWord.interactivePart.Length; i++)
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
            AvailableHints--;
            SaveGame();
        }
    }
}
