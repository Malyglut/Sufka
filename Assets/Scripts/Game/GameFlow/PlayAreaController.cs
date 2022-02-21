using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Controls;
using Sufka.Game.Validation;
using Sufka.Game.Words;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Sufka.Game.GameFlow
{
    public class PlayAreaController : MonoBehaviour
    {
        private const int FIRST_ATTEMPT_POINT_MULTIPLIER = 2;

        public event Action OnRoundOver;
        public event Action OnPointsUpdated;
        public event Action<int> OnPointsAwarded;
        public event Action OnRoundStarted;
        public event Action<int> OnWordGuessed;
        public event Action OnHintUsed;
        public event Action OnHintAdRequested;
        public event Action OnBackToMenuPopupRequested;

        [SerializeField]
        private GameObject _playAreaScreen;

        [SerializeField]
        private PlayArea _playArea;

        [SerializeField]
        private Keyboard _keyboard;

        [SerializeField]
        private GameSetupDatabase _gameSetupDatabase;

        [FoldoutGroup("Debug"), SerializeField, ReadOnly]
        private string _currentWord;

        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private Button _backToMenuButton;

        private readonly List<int> _guessedIndices = new List<int>();

        private GameSetup _currentGameSetup;
        private bool _initialized;

        public WordLength WordLength { get; private set; }
        public Word TargetWord { get; private set; }
        public bool HintUsed { get; private set; }

        public string TargetWordString => TargetWord.fullWord;

        public void StartGame(WordLength wordLength)
        {
            Initialize();

            WordLength = wordLength;
            _currentGameSetup = _gameSetupDatabase[WordLength];

            _playAreaScreen.SetActive(true);

            RandomWordRound();
        }

        public void StartGame(WordLength wordLength, Word targetWord, bool hintUsed)
        {
            Initialize();

            WordLength = wordLength;
            _currentGameSetup = _gameSetupDatabase[WordLength];

            _playAreaScreen.SetActive(true);

            SavedGameRound(targetWord, hintUsed);
        }

        private void SavedGameRound(Word targetWord, bool hintUsed)
        {
            TargetWord = targetWord;
            HintUsed = hintUsed;

            _guessedIndices.Clear();
            _currentWord = TargetWord.fullWord;

            _playArea.Initialize(_currentGameSetup.AttemptCount, _currentGameSetup.LetterCount, TargetWord);
            _keyboard.Reset(HintUsed);

            OnRoundStarted.Invoke();
        }

        private void Initialize()
        {
            if (!_initialized)
            {
                _playArea.OnCurrentRowFull += EnableEnter;
                _playArea.OnCurrentRowNotFull += DisableEnter;

                _keyboard.Initialize();
                _keyboard.OnKeyPress += HandleLetterInput;
                _keyboard.OnEnterPress += CheckWord;
                _keyboard.OnBackPress += HandleRemoveLetter;
                _keyboard.OnHintPress += GetHint;
                _keyboard.OnHintAdPress += RequestHintAd;

                _backToMenuButton.onClick.AddListener(RequestBackToMenuPopup);

                _initialized = true;
            }
        }

        private void RequestBackToMenuPopup()
        {
            OnBackToMenuPopupRequested.Invoke();
        }

        private void RequestHintAd()
        {
            OnHintAdRequested.Invoke();
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
            TargetWord = _currentGameSetup.WordAtlas.RandomWord();
            StartNewRound();
        }

        private void StartNewRound()
        {
            HintUsed = false;
            _guessedIndices.Clear();

            _currentWord = TargetWord.fullWord;

            _playArea.Initialize(_currentGameSetup.AttemptCount, _currentGameSetup.LetterCount, TargetWord);
            _keyboard.Reset(HintUsed);

            OnRoundStarted.Invoke();
        }

        private void CheckWord()
        {
            if (_playArea.ValidInput)
            {
                var result = WordValidator.Validate(TargetWord.interactivePart, _playArea.CurrentWord);

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
                    var pointsToAward = _currentGameSetup.AttemptCount - _playArea.Attempt;

                    if (_playArea.Attempt == 0)
                    {
                        pointsToAward *= FIRST_ATTEMPT_POINT_MULTIPLIER;
                    }

                    OnPointsAwarded.Invoke(pointsToAward);
                    OnWordGuessed.Invoke(_playArea.Attempt);

                    RandomWordRound();
                }
                else if (!result.FullMatch && _playArea.LastAttempt)
                {
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
            TargetWord = _currentGameSetup.WordAtlas.RandomNoun();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void AdjectiveRound()
        {
            TargetWord = _currentGameSetup.WordAtlas.RandomAdjective();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void VerbRound()
        {
            TargetWord = _currentGameSetup.WordAtlas.RandomVerb();
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
        private void GetHint()
        {
            if (HintUsed || _gameController.AvailableHints == 0)
            {
                return;
            }

            var possibleHints = new List<int>();

            for (var i = 0; i < TargetWord.interactivePart.Length; i++)
            {
                if (_guessedIndices.Contains(i))
                {
                    continue;
                }

                possibleHints.Add(i);
            }

            var hintIdx = possibleHints[Random.Range(0, possibleHints.Count)];
            var hintLetter = TargetWord.interactivePart[hintIdx];

            Debug.Log($"HINTING {hintLetter} AT INDEX {hintIdx}");

            _guessedIndices.Add(hintIdx);

            _keyboard.MarkGuessed(hintLetter);
            _playArea.MarkGuessed(hintIdx, hintLetter);

            HintUsed = true;
            OnHintUsed.Invoke();
        }

        public void Hide()
        {
            _playAreaScreen.SetActive(false);
        }
    }
}
