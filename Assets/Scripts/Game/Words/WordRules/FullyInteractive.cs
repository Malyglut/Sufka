using UnityEngine;

namespace Sufka.Game.Words.WordRules
{
    [CreateAssetMenu(fileName = "Fully Interactive", menuName = "Sufka/Word Rules/Fully Interactive")]
    public class FullyInteractive : WordRule
    {
        public override Word Apply(WordType wordType, string wordString)
        {
            return new Word(wordType, wordString, string.Empty);
        }
    }
}
