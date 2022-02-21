using System;

namespace Sufka.Game.Words
{
    [Serializable]
    public class Word
    {
        public readonly WordType wordType;
        public readonly string interactivePart;
        public readonly string nonInteractivePart;
        public readonly int nonInteractiveLength;
        public readonly string fullWord;

        public Word(WordType wordType, string interactivePart, string nonInteractivePart)
        {
            this.wordType = wordType;
            this.interactivePart = interactivePart.ToUpper();
            this.nonInteractivePart = nonInteractivePart.ToUpper();
            fullWord = this.interactivePart + this.nonInteractivePart;
            nonInteractiveLength = nonInteractivePart.Length;
        }
    }
}
