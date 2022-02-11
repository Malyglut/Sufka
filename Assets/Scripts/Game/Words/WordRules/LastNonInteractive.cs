using UnityEngine;

namespace Sufka.Game.Words.WordRules
{
    [CreateAssetMenu(fileName = "Last Non Interactive", menuName = "Sufka/Word Rules/Last Non Interactive")]
    public class LastNonInteractive : WordRule
    {
        protected string _interactivePart;
        protected string _nonInteractivePart;

        public override Word Apply(WordType wordType, string wordString)
        {
            SplitWord(wordString);
            return new Word(wordType, _interactivePart, _nonInteractivePart);
        }

        protected void SplitWord(string wordString)
        {
            _interactivePart = wordString.Substring(0, wordString.Length - 1);
            _nonInteractivePart = wordString.Substring(wordString.Length - 1, 1);
        }
    }
}
