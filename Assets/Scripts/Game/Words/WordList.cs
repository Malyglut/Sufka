using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Game.Words
{
    public class WordList : ScriptableObject
    {
        [SerializeField]
        private List<string> _words = new List<string>();
        
        public void Initialize(IEnumerable<string> words)
        {
            _words.AddRange(words);
        }

        public string RandomWord()
        {
            return _words[Random.Range(0, _words.Count)];
        }
    }
}
