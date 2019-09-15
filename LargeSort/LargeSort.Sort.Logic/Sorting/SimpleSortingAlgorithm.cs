using System;
using System.Collections.Generic;

namespace LargeSort.Sort.Logic.Sorting
{
    internal class SimpleSortingAlgorithm : ISortingAlgorithm
    {
        public void Sort(List<string> toSort)
        {
            toSort.Sort(StringComparer.Ordinal);
        }
    }
}