using System;
using Random = UnityEngine.Random;

namespace Sufka.Words.WordRules
{
    [Serializable]
    public class LastNonInteractiveWithReplace : LastNonInteractive
    {
        private const string REPLACE_WITH = "a";

        public override Word Apply(WordType wordType, string wordString)
        {
            SplitWord(wordString);

            if (Random.value > .5f)
            {
                _nonInteractivePart = REPLACE_WITH;
            }

            return new Word(wordType, _interactivePart, _nonInteractivePart);
        }
    }
}
