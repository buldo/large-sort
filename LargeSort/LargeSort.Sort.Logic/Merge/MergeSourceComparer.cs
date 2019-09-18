using System.Collections.Generic;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MergeSourceComparer : IComparer<MergeSource>
    {
        private static CompositeStringComparer _comparer = new CompositeStringComparer();

        public int Compare(MergeSource x, MergeSource y)
        {
            var result = _comparer.Compare(x.Current, y.Current);
            if (result == 0)
            {
                result = string.CompareOrdinal(x.FileName, y.FileName);
            }

            return result;
        }
    }
}
