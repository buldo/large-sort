using System;
using System.IO;
using LargeSort.FileSystem;
using LargeSort.IntegrationTests.Assertions;
using LargeSort.Sort.Logic;
using NUnit.Framework;
using Serilog;

namespace LargeSort.IntegrationTests
{
    public class Tests
    {
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }

        [Test]
        public void GenerateAndSortSingleChunkSuccess()
        {
            using (var writer = new FileStreamWriter("random", FileMode.Create))
            {
                var generator = new Generator.Logic.Generator(
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "dictionary.txt"),
                    writer);
                generator.Generate(256 * 1048576);
            }

            var sorter = new Sorter("random", "sorted", SortingAlgorithms.Simple, _logger);
            var sortedDir = Directory.CreateDirectory("temp");
            foreach (var fileInfo in sortedDir.GetFiles())
            {
                fileInfo.Delete();
            }

            sorter.Sort(sortedDir, false);

            foreach (var file in sortedDir.GetFiles())
            {
                SortingAssert.FileSorted(file.FullName, StringComparer.Ordinal);
            }
        }
    }
}