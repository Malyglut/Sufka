using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Keyboard
{
    public class KeyboardLayoutData : ScriptableObject
    {
        [SerializeField]
        private List<CharList> _keys = new List<CharList>();
        
        public List<CharList> Keys => _keys;
        
        #if UNITY_EDITOR
        public void Initialize(char[,] keys)
        {
            var width = keys.GetLength(0);
            var height = keys.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                _keys.Add(new CharList());
            }
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    _keys[h].chars.Add(keys[w, h]);
                }
            }
        }
        #endif
    }
}
