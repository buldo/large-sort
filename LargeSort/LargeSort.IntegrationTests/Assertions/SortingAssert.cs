using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace LargeSort.IntegrationTests.Assertions
{
    internal class SortingAssert
    {
        public static void FileSorted(string path, IComparer<string> comparer)
        {
            string prevLine = string.Empty;
            using var reader = new StreamReader(path);
            string currentLine = null;
            while ((currentLine = reader.ReadLine()) != null)
            {
                Assert.LessOrEqual(comparer.Compare(prevLine, currentLine), 0);
                prevLine = currentLine;
            }
        }
    }
}
