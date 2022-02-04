using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Words
{
    [Serializable, CreateAssetMenu(fileName = "Word Atlas", menuName = "Sufka/Word Atlas")]
    public class WordAtlas : ScriptableObject
    {
        [SerializeField]
        private int _wordLength = 5;
        
        [SerializeField]
        private List<string> _words = new List<string>();

        [Button, PropertyOrder(-100)]
        private void ValidateWords()
        {
            var whitespaceRegex = new Regex("\\s");
            
            for (int i = _words.Count-1; i >=0; i--)
            {
                var word = _words[i];

                if (word.Length != _wordLength || whitespaceRegex.IsMatch(word))
                {
                    _words.RemoveAt(i);
                }
            }
        }

        public string RandomWord()
        {
            return _words[Random.Range(0, _words.Count)].ToUpper();
        }
    }
}
