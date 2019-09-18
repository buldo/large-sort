using System;

namespace LargeSort.Sort.Logic
{
    public class CompositeString
    {
        public void Init(string original)
        {
            Original = original;

            var wordIndex = original.IndexOf(". ", StringComparison.Ordinal) + 2;

            Word = original.Substring(wordIndex);

            Number = int.Parse(original.Substring(0, wordIndex - 2));
        }

        public string Original { get; set; }

        public string Word { get; set; }

        public int Number { get; set; }
    }
}