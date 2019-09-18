using System.Collections.Generic;

namespace LargeSort.Sort.Logic
{
    class CompositeStringComparer : IComparer<CompositeString>
    {
        public int Compare(CompositeString x, CompositeString y)
        {
            return CompareTo(x, y);
        }

        private static int CompareTo(CompositeString x, CompositeString y)
        {
            int result = string.CompareOrdinal(x.Word, x.Word);

            if (result == 0)
            {
                result = x.Number.CompareTo(y.Number);
            }

            return result;
        }
    }
}