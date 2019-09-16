using System;
using System.Collections.Generic;
using System.Text;
using CSharpStringSort;

namespace LargeSort.Sort.Logic.Sorting
{
    internal class SedgewicSortingAlgorithm : ISortingAlgorithm
    {
        public void Sort(List<string> toSort)
        {
            Sedgewick.InPlaceSort(toSort,0, toSort.Count,0);
        }
    }
}
