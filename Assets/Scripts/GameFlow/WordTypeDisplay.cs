using System.Collections.Generic;
using Sufka.Words;
using TMPro;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class WordTypeDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private GameController _gameController;

        private readonly Dictionary<WordType, string> _wordTypeStrings = new Dictionary<WordType, string>
                                                                         {
                                                                             {WordType.Noun, "RZECZOWNIK"},
                                                                             {WordType.Adjective, "PRZYMIOTNIK"},
                                                                             {WordType.Verb, "CZASOWNIK"}
                                                                         };

        private void Awake()
        {
            _gameController.OnRoundStarted += Refresh;
        }

        private void Refresh(Word word)
        {
            var wordTypeString = _wordTypeStrings[word.wordType];
            _text.SetText(wordTypeString);
        }
    }
}
