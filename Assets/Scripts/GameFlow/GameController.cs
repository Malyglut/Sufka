using System;
using Sirenix.OdinInspector;
using Sufka.Controls;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;

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

        [FoldoutGroup("Debug"), SerializeField, ReadOnly]
        private string _currentWord;

        private void Start()
        {
            _playArea.OnCurrentRowFull += EnableEnter;
            _playArea.OnCurrentRowNotFull += DisableEnter;

            _keyboard.Initialize();
            _keyboard.OnKeyPress += HandleLetterInput;
            _keyboard.OnEnterPress += CheckWord;
            _keyboard.OnBackPress += HandleRemoveLetter;

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
            _currentWord = _targetWord.interactivePart + _targetWord.nonInteractivePart;
            
            _playArea.Initialize(_attemptCount, _letterCount, _targetWord);
            _keyboard.Reset();
        }

        private void CheckWord()
        {
            if (_playArea.ValidInput)
            {
                var result = WordValidator.Validate(_targetWord.interactivePart, _playArea.CurrentWord);

                Debug.Log(result);

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
        
    }
}
