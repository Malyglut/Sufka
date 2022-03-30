using System.Collections.Generic;
using Sufka.Game.Words;
using TMPro;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class WordTypeDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private PlayAreaController _gameController;

        private readonly Dictionary<WordType, string> _wordTypeStrings = new Dictionary<WordType, string>
                                                                         {
                                                                             {WordType.Noun, "RZECZOWNIK"},
                                                                             {WordType.Adjective, "PRZYMIOTNIK"},
                                                                             {WordType.Verb, "CZASOWNIK"}
                                                                         };

        private void Awake()
        {
            if(_gameController!=null)
            {
                _gameController.OnRoundStarted += Refresh;
            }
        }

        private void Refresh()
        {
            var wordTypeString = _wordTypeStrings[_gameController.TargetWord.wordType];
            _text.SetText(wordTypeString);
        }
    }
}
