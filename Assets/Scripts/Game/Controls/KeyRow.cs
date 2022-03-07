using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Controls
{
    public class KeyRow : MonoBehaviour
    {
        public event Action<char> OnKeyPress;

        [SerializeField]
        private LetterKey _letterKeyPrefab;

        [SerializeField]
        private KeyboardOffset _keyOffsetPrefab;

        [SerializeField]
        private Transform _parent;

        private readonly Dictionary<char, LetterKey> _keys = new Dictionary<char, LetterKey>();
        private readonly List<KeyboardOffset> _keyboardOffsets = new List<KeyboardOffset>();

        public LetterKey this[char letter] => _keys[letter];

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

        public void Reset()
        {
            foreach (var key in _keys.Values)
            {
                key.Reset();
            }
        }

        public void Offset(int keyDeficit)
        {
            if (keyDeficit <= 0)
            {
                return;
            }

            if (keyDeficit % 2 == 1)
            {
                keyDeficit--;

                OffsetOnBothSides(true);
            }

            for (var i = 0; i < keyDeficit / 2; i++)
            {
                OffsetOnBothSides(false);
            }
        }

        private void OffsetOnBothSides(bool isHalf)
        {
            var offsetLeft = Instantiate(_keyOffsetPrefab, _parent);
            var offsetRight = Instantiate(_keyOffsetPrefab, _parent);

            offsetLeft.transform.SetAsFirstSibling();
            offsetRight.transform.SetAsLastSibling();

            if (isHalf)
            {
                offsetLeft.Half();
                offsetRight.Half();
            }

            _keyboardOffsets.Add(offsetLeft);
            _keyboardOffsets.Add(offsetRight);
        }
    }
}
