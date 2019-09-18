using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LargeSort.Sort.Logic.PreSorting;
using Serilog;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly ILogger _logger;

        public Sorter(string inputFile, ILogger logger)
        {
            _inputFile = inputFile;
            _logger = logger;
        }

        public void Sort(string outFile, int count)
        {
            var tempFolder = Path.Combine(Path.GetDirectoryName(outFile), Path.GetRandomFileName());

            var preSorter = new PreSorter(_inputFile);
            var watch = Stopwatch.StartNew();
            preSorter.PreSort(tempFolder, count);
            watch.Stop();
            _logger.Information($"Presorting for {watch.Elapsed.ToString()}");
        }

    }
}