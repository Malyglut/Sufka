using UnityEngine;

namespace Sufka.Game.Words.WordRules
{
    public abstract class WordRule : ScriptableObject
    {
        public abstract Word Apply(WordType wordType, string wordString);
    }
}
