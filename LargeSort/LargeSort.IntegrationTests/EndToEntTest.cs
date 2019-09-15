using System;
using System.IO;
using LargeSort.FileSystem;
using LargeSort.IntegrationTests.Assertions;
using LargeSort.Sort.Logic;
using NUnit.Framework;

namespace LargeSort.IntegrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateAndSortSingleChunkSuccess()
        {
            using (var writer = new FileStreamWriter("random", FileMode.Create))
            {
                var generator = new Generator.Logic.Generator(
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "dictionary.txt"),
                    writer);
                generator.Generate(1);
            }

            var sorter = new Sorter("random", "sorted", SortingAlgorithms.Simple);
            var sortedDir = Directory.CreateDirectory("temp");
            foreach (var fileInfo in sortedDir.GetFiles())
            {
                fileInfo.Delete();
            }

            sorter.Sort(sortedDir);

            foreach (var file in sortedDir.GetFiles())
            {
                SortingAssert.FileSorted(file.FullName, StringComparer.Ordinal);
            }
        }
    }
}