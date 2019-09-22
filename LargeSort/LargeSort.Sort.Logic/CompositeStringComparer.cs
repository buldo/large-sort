using System.Collections.Generic;

namespace LargeSort.Sort.Logic
{
    public class CompositeStringComparer : IComparer<CompositeString>
    {
        public int Compare(CompositeString x, CompositeString y)
        {
            return CompareTo(x, y);
        }

        private static int CompareTo(CompositeString x, CompositeString y)
        {
            int result = string.CompareOrdinal(x.Word, y.Word);

            if (result == 0)
            {
                result = x.Number.CompareTo(y.Number);
            }

            return result;
        }
    }
}