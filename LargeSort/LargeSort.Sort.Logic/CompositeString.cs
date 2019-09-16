using System;
using System.Collections.Generic;
using System.Text;

namespace LargeSort.Sort.Logic
{
    public class CompositeString : IComparable<CompositeString>
    {
        public CompositeString(string original)
        {
            Original = original;
            WordIndex = original.IndexOf(". ", StringComparison.Ordinal) + 2;
            Number = long.Parse(original.AsSpan(0, WordIndex - 2));
        }

        public int WordIndex { get; }

        public string Original { get; }

        public long Number { get; }

        public int CompareTo(CompositeString other)
        {
            var thisLen = Original.Length - WordIndex;
            var otherLen = other.Original.Length - other.WordIndex;
            int result = string.Compare(
                Original, WordIndex,
                other.Original, other.WordIndex,
                thisLen < otherLen ? thisLen : otherLen);

            if (result == 0)
            {
                result = Number.CompareTo(other.Number);
            }


            return result;
        }
    }
}
