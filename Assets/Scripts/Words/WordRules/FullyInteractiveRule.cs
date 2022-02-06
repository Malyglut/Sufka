using System;

namespace Sufka.Words.WordRules
{
    [Serializable]
    public class FullyInteractiveRule : WordRule
    {
        public override Word Apply(string wordString)
        {
            return new Word(wordString, string.Empty);
        }
    }
}
