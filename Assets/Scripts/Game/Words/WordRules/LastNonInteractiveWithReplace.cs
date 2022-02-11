using UnityEngine;

namespace Sufka.Game.Words.WordRules

{
    [CreateAssetMenu(fileName = "Last Non Interactive With Replace",
                     menuName = "Sufka/Word Rules/Last Non Interactive With Replace")]
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
