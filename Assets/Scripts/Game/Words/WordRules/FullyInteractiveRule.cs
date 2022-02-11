using System;

namespace Sufka.Game.Words.WordRules
{
    [Serializable]
    public class FullyInteractiveRule : WordRule
    {
        public override Word Apply(WordType wordType, string wordString)
        {
            return new Word(wordType, wordString, string.Empty);
        }
    }
}
