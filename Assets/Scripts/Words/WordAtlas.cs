using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Words
{
    [CreateAssetMenu(fileName = "Word Atlas", menuName = "Sufka/Word Atlas")]
    public class WordAtlas : SerializedScriptableObject
    {
        [SerializeField]
        private int _wordLength = 5;

        [SerializeField]
        private Dictionary<WordType, WordList> _words = new Dictionary<WordType, WordList>
                                                        {
                                                            {WordType.Noun, null},
                                                            {WordType.Adjective, null},
                                                            {WordType.Verb, null}
                                                        };

        [SerializeField]
        private WordRuleSet _ruleSet;

        public Word RandomWord()
        {
            var wordTypes = _words.Keys.ToList();
            var wordType = wordTypes[Random.Range(0, wordTypes.Count)];
            return RandomWord(wordType);
        }

        private Word RandomWord(WordType wordType)
        {
            var wordList = _words[wordType];
            var wordString = wordList.RandomWord();

            return _ruleSet.Apply(wordType, wordString);
        }

        public Word RandomNoun()
        {
            return RandomWord(WordType.Noun);
        }

        public Word RandomAdjective()
        {
            return RandomWord(WordType.Adjective);
        }

        public Word RandomVerb()
        {
            return RandomWord(WordType.Verb);
        }
    }
}
