using System;
using System.Collections.Generic;
using System.Text;
using LargeSort.Sort.Logic.Sorting;

namespace LargeSort.Sort.Logic
{
    public static class SortingAlgorithms
    {
        public static ISortingAlgorithm Simple { get; } = new SimpleSortingAlgorithm();

        public static ISortingAlgorithm Sedgewic { get; } = new SedgewicSortingAlgorithm();
    }
}
