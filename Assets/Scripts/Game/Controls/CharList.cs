using System;
using System.Collections.Generic;

namespace Sufka.Controls
{
    [Serializable]
    public class CharList
    {
        public List<char> chars = new List<char>();
        public char this[int idx] => chars[idx];
    }
}