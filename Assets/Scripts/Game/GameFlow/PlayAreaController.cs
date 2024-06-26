using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Controls;
using Sufka.Game.Persistence;
using Sufka.Game.Utility;
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

        public event Action OnRoundOver = EventUtility.Empty;
        public event Action OnGameProgressUpdated = EventUtility.Empty;
        public event Action OnPointsUpdated = EventUtility.Empty;
        public event Action<int> OnPointsAwarded = EventUtility.Empty;
        public event Action OnRoundStarted = EventUtility.Empty;
        public event Action<int> OnWordGuessed = EventUtility.Empty;
        public event Action<int, int, int, int> OnLetterStatisticsUpdated = EventUtility.Empty;
        public event Action OnHintUsed = EventUtility.Empty;
        public event Action OnHintAdRequested = EventUtility.Empty;
        public event Action OnBackToMenuPopupRequested = EventUtility.Empty;
        public event Action OnDailyTasksScreenRequested = EventUtility.Empty;

        [SerializeField]
        private GameObject _playAreaScreen;

        [SerializeField]
        private PlayArea _playArea;

        [SerializeField]
        private Keyboard _keyboard;

        [SerializeField]
        private RoundOverDisplay _roundOverMessage;

        [SerializeField]
        private GameSetupDatabase _gameSetupDatabase;

        [SerializeField]
        private ScoreDisplay _scoreDisplay;

#if UNITY_EDITOR
        [FoldoutGroup("Debug"), SerializeField, ReadOnly]
        private string _currentWord;
#endif

        [SerializeField]
        private Button _backToMenuButton;
        
        [SerializeField]
        private Button _dailyTasksButton;

        private GameSetup _currentGameSetup;
        private bool _initialized;

        public GameMode GameMode { get; private set; }
        public Word TargetWord { get; private set; }
        public bool HintUsed { get; private set; }
        public int HintIdx { get; private set; }
        public int HintRow { get; private set; }
        public List<int> GuessedIndices { get; private set; } = new List<int>();

        public List<List<LetterResult>> FilledLetters => _playArea.GetFilledLetters();
        public int CurrentAttempt => _playArea.CurrentAttempt;

        public void StartGame(GameMode gameMode)
        {
            Initialize();

            GameMode = gameMode;
            _currentGameSetup = _gameSetupDatabase[GameMode];

            _playAreaScreen.SetActive(true);
            _scoreDisplay.Refresh();

            RandomWordRound();
        }

        public void StartGame(GameMode gameMode, GameInProgressSaveData data)
        {
            Initialize();

            GameMode = gameMode;
            HintIdx = data.hintIdx;
            HintRow = data.hintRow;
            GuessedIndices = new List<int>(data.guessedIndices);
            _currentGameSetup = _gameSetupDatabase[GameMode];

            _playAreaScreen.SetActive(true);
            _scoreDisplay.Refresh();

            SavedGameRound(data);
        }

        private void SavedGameRound(GameInProgressSaveData data)
        {
            TargetWord = data.targetWord;
            HintUsed = data.hintUsed;

            GuessedIndices.Clear();

#if UNITY_EDITOR
            _currentWord = TargetWord.fullWord;
#endif

            _playArea.Initialize(_currentGameSetup.AttemptCount, _currentGameSetup.LetterCount, TargetWord);

            if (HintUsed)
            {
                _playArea.MarkHint(HintRow, HintIdx, data.filledLetters[HintRow][HintIdx].letter);
            }

            _playArea.RestoreLetters(data.filledLetters);
            _keyboard.Restore(HintUsed);
            _keyboard.RestoreKeys(data.filledLetters);

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
                _dailyTasksButton.onClick.AddListener(RequestDailyTasksScreen);

                _scoreDisplay.Initialize();

                _roundOverMessage.Initialize();
                _roundOverMessage.OnNextRoundRequested += RandomWordRound;

                _initialized = true;
            }
        }

        private void RequestDailyTasksScreen()
        {
            OnDailyTasksScreenRequested.Invoke();
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

        public void TutorialDisableEnter()
        {
            DisableEnter();
        }

        private void RandomWordRound()
        {
            TargetWord = _currentGameSetup.WordAtlas.RandomWord();
            StartNewRound();
        }

        private void StartNewRound()
        {
            HintUsed = false;
            GuessedIndices.Clear();

#if UNITY_EDITOR
            _currentWord = TargetWord.fullWord;
#endif

            _playArea.Initialize(_currentGameSetup.AttemptCount, _currentGameSetup.LetterCount, TargetWord);
            _keyboard.Restore(HintUsed);
            _roundOverMessage.Hide();

            OnRoundStarted.Invoke();
            OnGameProgressUpdated.Invoke();
        }

        private void CheckWord()
        {
            if (_playArea.ValidInput)
            {
                var result = WordValidator.Validate(TargetWord.interactivePart, _playArea.CurrentWord);

                OnLetterStatisticsUpdated.Invoke(result.CorrectLetterCount,
                                                 result.CorrectSpotCount,
                                                 _playArea.TypedLetters,
                                                 _playArea.RemovedLetters);

#if UNITY_EDITOR
                Debug.Log(result);
#endif

                foreach (var idx in result.GuessedIndices)
                {
                    if (!GuessedIndices.Contains(idx))
                    {
                        GuessedIndices.Add(idx);
                    }
                }

                if (result.FullMatch)
                {
                    var pointsToAward = _currentGameSetup.AttemptCount - _playArea.CurrentAttempt;

                    if (_playArea.CurrentAttempt == 0)
                    {
                        pointsToAward *= FIRST_ATTEMPT_POINT_MULTIPLIER;
                    }

                    OnPointsAwarded.Invoke(pointsToAward);
                    OnWordGuessed.Invoke(_playArea.CurrentAttempt);

                    _playArea.Display(result);

                    _keyboard.Hide();
                    _roundOverMessage.ShowWin(pointsToAward);
                }
                else if (!result.FullMatch && _playArea.LastAttempt)
                {
                    OnRoundOver.Invoke();

                    _playArea.Display(result);
                    _keyboard.Hide();
                    _roundOverMessage.ShowLoss(_currentGameSetup.LetterCount, TargetWord);
                }
                else
                {
                    _playArea.Display(result);
                    _keyboard.Refresh(result);

                    OnGameProgressUpdated.Invoke();
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

        public void Hide()
        {
            _playAreaScreen.SetActive(false);
        }

        public void RefreshHints()
        {
            _keyboard.RefreshHints();
        }

        public void RefreshPoints(int pointsToAward)
        {
            _scoreDisplay.Refresh(pointsToAward);
        }

#if UNITY_EDITOR
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
        private void CustomVerbRound(Word word)
        {
            TargetWord = word;
            StartNewRound();
        }

        [FoldoutGroup("Debug"), Button]
#endif
        private void GetHint()
        {
            if (HintUsed)
            {
                return;
            }

            var possibleHints = new List<int>();

            for (var i = 0; i < TargetWord.interactivePart.Length; i++)
            {
                if (GuessedIndices.Contains(i))
                {
                    continue;
                }

                possibleHints.Add(i);
            }

            HintIdx = possibleHints[Random.Range(0, possibleHints.Count)];
            var hintLetter = TargetWord.interactivePart[HintIdx];

            Debug.Log($"HINTING {hintLetter} AT INDEX {HintIdx}");

            GuessedIndices.Add(HintIdx);

            _keyboard.MarkGuessed(hintLetter);
            _playArea.MarkGuessed(HintIdx, hintLetter);
            HintRow = _playArea.CurrentAttempt;

            HintUsed = true;
            OnHintUsed.Invoke();
            OnGameProgressUpdated.Invoke();
        }
    }
}
