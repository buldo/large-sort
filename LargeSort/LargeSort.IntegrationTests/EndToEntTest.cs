using System;
using System.Diagnostics;
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
            using (var writer = new StreamWriter(randomFileName, false))
            {
                var generator = new Generator.Logic.Generator(
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "dictionary.txt"),
                    writer);
                generator.Generate(1024 * 1048576);
            }

            var sortWatch = Stopwatch.StartNew();

            const string sortedFileName = "sorted";
            File.Delete(sortedFileName);
            var sortedDir = Directory.CreateDirectory(TestContext.CurrentContext.Test.Name);
            var sorter = new Sorter(randomFileName, _logger);
            foreach (var fileInfo in sortedDir.GetFiles())
            {
                fileInfo.Delete();
            }

            using (var writer = new FileStreamWriter(sortedFileName, false))
            {
                sorter.Sort(sortedDir.FullName, 8);
                writer.Flush();
            }

            SortingAssert.FileSorted(sortedFileName, StringComparer.Ordinal);

            var expectedLines = File.ReadAllLines(randomFileName).Length;
            var actualLines = File.ReadAllLines(sortedFileName).Length;
            Assert.AreEqual(expectedLines, actualLines);

            sortWatch.Stop();
            _logger.Information($"Sorted for {sortWatch.Elapsed.ToString()}");
        }
    }
}