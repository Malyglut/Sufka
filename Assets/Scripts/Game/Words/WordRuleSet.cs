using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Words.WordRules;
using UnityEngine;

namespace Sufka.Game.Words
{
    [CreateAssetMenu(fileName = "Word Rule Set", menuName = "Sufka/Word Rule Set", order = 0)]
    public class WordRuleSet : SerializedScriptableObject
    {
        [SerializeField]
        private readonly Dictionary<WordType, WordRule> _rules = new Dictionary<WordType, WordRule>
                                                                 {
                                                                     {WordType.Noun, null},
                                                                     {WordType.Adjective, null},
                                                                     {WordType.Verb, null}
                                                                 };

        public Word Apply(WordType wordType, string wordString)
        {
            return _rules[wordType].Apply(wordType, wordString);
        }
    }
}
