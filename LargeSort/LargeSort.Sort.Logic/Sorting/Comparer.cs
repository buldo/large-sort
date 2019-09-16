using System;
using System.Collections.Generic;
using System.Text;

namespace LargeSort.Sort.Logic.Sorting
{
    class Comparer: IComparer<CompositeString>
    {
        public int Compare(CompositeString x, CompositeString y)
        {
            return x.CompareTo(y);
        }
    }
}
