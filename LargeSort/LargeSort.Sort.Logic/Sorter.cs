using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LargeSort.Sort.Logic.Merge;
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
            Directory.CreateDirectory(tempFolder);

            var watch = Stopwatch.StartNew();
            var preSorter = new PreSorter(_inputFile);
            preSorter.PreSort(tempFolder, count);
            watch.Stop();
            Console.WriteLine($"Presorting for {watch.Elapsed.ToString()}");

            watch = Stopwatch.StartNew();
            var merge = new MultiFilesMerge(tempFolder);
            merge.MultiLevelMerge(outFile);
            watch.Stop();
            Console.WriteLine($"Merging for {watch.Elapsed.ToString()}");
        }

    }
}