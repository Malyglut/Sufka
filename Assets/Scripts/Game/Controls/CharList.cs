using System;
using System.Linq;
using System.Collections.Generic;

namespace Sufka.Game.Controls
{
    [Serializable]
    public class CharList
    {
        public List<char> chars = new List<char>();
        public char this[int idx] => chars[idx];

        public int CharCount => chars.Count(c => c != '\0');
    }
}
