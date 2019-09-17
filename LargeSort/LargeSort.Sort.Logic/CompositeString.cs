using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace LargeSort.Sort.Logic
{
    public class CompositeString : IComparable<CompositeString>
    {
        public CompositeString(string original)
        {
            Original = original;
            WordIndex = original.IndexOf(". ", StringComparison.Ordinal) + 2;
            try
            {
                Number = long.Parse(original.AsSpan(0, WordIndex - 2));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, $"Original: {Original}; WordIndex:{WordIndex}");
                throw;
            };
        }

        public int WordIndex { get; }

        public string Original { get; }

        public long Number { get; }

        public int CompareTo(CompositeString other)
        {
            int result = Original.AsSpan(WordIndex).CompareTo(other.Original.AsSpan(other.WordIndex), StringComparison.Ordinal);

            if (result == 0)
            {
                result = Number.CompareTo(other.Number);
            }


            return result;
        }
    }
}
