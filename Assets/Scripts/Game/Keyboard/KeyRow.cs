using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Keyboard
{
    public class KeyRow : MonoBehaviour
    {
        public event Action<char> OnKeyPress;
        
        [SerializeField]
        private LetterKey _letterKeyPrefab;

        [SerializeField]
        private Transform _parent;

        private Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();

        public void AddKey(char character)
        {
            var key = Instantiate(_letterKeyPrefab, _parent);
            key.Initialize(character);
            key.OnPress += HandleKeyPress;
            _keys.Add(character, key);
        }

        private void HandleKeyPress(char letter)
        {
            OnKeyPress.Invoke(letter);
        }

        public bool Contains(char letter)
        {
            return _keys.ContainsKey(letter);
        }

        public LetterKey this[char letter] => _keys[letter];

        public void Reset()
        {
            foreach (var key in _keys.Values)
            {
                key.Reset();
            }
        }
    }
}
