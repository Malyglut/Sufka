using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sufka.Game.Words
{
    [CreateAssetMenu(fileName = "Word Rule Set", menuName = "Sufka/Word Rule Set", order = 0)]
    public class WordRuleSet : ScriptableObject
    {
        [SerializeField]
        private List<WordTypeRule> _wordTypeRules = new List<WordTypeRule>();

        public Word Apply(WordType wordType, string wordString)
        {
            return _wordTypeRules.First(rule => rule.wordType == wordType).wordRule.Apply(wordType, wordString);
        }
    }
}
