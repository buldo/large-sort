using System;
using System.Collections.Generic;

namespace LargeSort.Sort.Logic.Sorting
{
    internal class SimpleSortingAlgorithm : ISortingAlgorithm
    {
        private readonly Comparer _comparer = new Comparer();
        public void Sort(List<CompositeString> toSort)
        {
            toSort.Sort(_comparer);
        }
    }
}