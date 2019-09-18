using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LargeSort.Sort.Logic;
using NUnit.Framework;

namespace LargeSort.IntegrationTests.Assertions
{
    internal class SortingAssert
    {
        public static void FileSorted(string path, CompositeStringComparer comparer)
        {
            CompositeString prevComposite = new CompositeString();
            Assert.True(File.Exists(path));
            using (var reader = new StreamReader(path))
            {
                string currentLine = null;
                CompositeString currentComposite = new CompositeString();
                while ((currentLine = reader.ReadLine()) != null)
                {
                    currentComposite.Init(currentLine);
                    Assert.LessOrEqual(comparer.Compare(prevComposite, currentComposite), 0);
                    prevComposite = currentComposite;
                }
            }
        }
    }
}
