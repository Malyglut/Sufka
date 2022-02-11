using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Words
{
    [CreateAssetMenu(fileName = "Word Atlas", menuName = "Sufka/Word Atlas")]
    public class WordAtlas : ScriptableObject
    {
        [SerializeField]
        private List<WordTypeList> _wordLists = new List<WordTypeList>();

        [SerializeField]
        private WordRuleSet _ruleSet;

        public Word RandomWord()
        {
            var wordType = _wordLists[Random.Range(0, _wordLists.Count)].wordType;
            return RandomWord(wordType);
        }

        private Word RandomWord(WordType wordType)
        {
            var wordList = _wordLists.First(words => words.wordType == wordType).wordList;
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
