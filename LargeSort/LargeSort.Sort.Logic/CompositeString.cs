using System;

namespace LargeSort.Sort.Logic
{
    public class CompositeString : IComparable<CompositeString>
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

        public int CompareTo(CompositeString other)
        {
            int result = StringComparer.Ordinal.Compare(Word, other.Word);

            if (result == 0)
            {
                result = Number.CompareTo(other.Number);
            }

            return result;
        }
    }
}