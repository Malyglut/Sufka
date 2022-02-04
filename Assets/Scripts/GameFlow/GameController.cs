using System;
using Sufka.Controls;
using Sufka.Validation;
using Sufka.Words;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class GameController : MonoBehaviour
    {
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

        private string _targetWord;
        
        private void Start()
        {
            _playArea.Initialize(_attemptCount, _letterCount);
            
            _keyboard.Initialize();
            _keyboard.OnKeyPress += HandleLetterInput;
            _keyboard.OnEnterPress += CheckWord;
            _keyboard.OnBackPress += HandleRemoveLetter;

            StartNewRound();
        }

        private void StartNewRound()
        {
            _playArea.Reset();
            _keyboard.Reset();

            _targetWord = _wordAtlas.RandomWord();
        }

        private void CheckWord()
        {
            if (_playArea.ValidInput)
            {
                var result = WordValidator.Validate(_targetWord, _playArea.CurrentWord);

                Debug.Log(result);

                if (result.FullMatch)
                {
                    Debug.Log("WIN!");
                    StartNewRound();
                }
                else if (!result.FullMatch && _playArea.LastAttempt)
                {
                    Debug.Log("LOSE!");
                    StartNewRound();
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
    }
}