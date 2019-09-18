using System.Collections.Generic;

namespace LargeSort.Sort.Logic
{
    class CompositeStringComparer : IComparer<CompositeString>
    {
        public int Compare(CompositeString x, CompositeString y)
        {
            return x.CompareTo(x);
        }
    }
}