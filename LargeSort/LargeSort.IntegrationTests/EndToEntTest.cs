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
        public void GenerateAndSortMultiChunkSuccess()
        {
            const string randomFileName = "random";
            using (var writer = new FileStreamWriter(randomFileName, FileMode.Create))
            {
                var generator = new Generator.Logic.Generator(
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "dictionary.txt"),
                    writer);
                generator.Generate(256 * 1048576);
            }

            const string sortedFileName = "sorted";
            File.Delete(sortedFileName);
            var sortedDir = Directory.CreateDirectory(TestContext.CurrentContext.Test.Name);
            var sorter = new Sorter(randomFileName, sortedDir.FullName, SortingAlgorithms.Simple, _logger);
            foreach (var fileInfo in sortedDir.GetFiles())
            {
                fileInfo.Delete();
            }

            using (var writer = new FileStreamWriter(sortedFileName, FileMode.Create))
            {
                sorter.Sort(64 * 1048576, writer);
            }

            SortingAssert.FileSorted(sortedFileName, StringComparer.Ordinal);

            var expectedLines = File.ReadAllLines(randomFileName).Length;
            var actualLines = File.ReadAllLines(sortedFileName);
            Assert.AreEqual(expectedLines, actualLines);
        }
    }
}