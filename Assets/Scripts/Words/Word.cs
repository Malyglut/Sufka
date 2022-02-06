namespace Sufka.Words
{
    public class Word
    {
        public readonly string interactivePart;
        public readonly string nonInteractivePart;
        public readonly int nonInteractiveLength;

        public Word(string interactivePart, string nonInteractivePart)
        {
            this.interactivePart = interactivePart.ToUpper();
            this.nonInteractivePart = nonInteractivePart.ToUpper();
            nonInteractiveLength = nonInteractivePart.Length;
        }
    }
}
