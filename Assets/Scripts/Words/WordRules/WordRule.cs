using System;

namespace Sufka.Words.WordRules
{
    [Serializable]
    public abstract class WordRule
    {
        public abstract Word Apply(WordType wordType, string wordString);
    }
}
