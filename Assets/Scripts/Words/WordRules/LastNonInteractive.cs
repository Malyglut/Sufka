using System;

namespace Sufka.Words.WordRules
{
    [Serializable]
    public class LastNonInteractive : WordRule
    {
        protected string _interactivePart;
        protected string _nonInteractivePart;
        
        public override Word Apply(string wordString)
        {
            SplitWord(wordString);
            return new Word(_interactivePart, _nonInteractivePart);
        }

        protected void SplitWord(string wordString)
        {
            _interactivePart =wordString.Substring(0, wordString.Length - 1);
            _nonInteractivePart = wordString.Substring(wordString.Length - 1, 1);
        }
    }
}
