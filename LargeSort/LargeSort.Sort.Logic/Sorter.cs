using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using LargeSort.FileSystem;
using LargeSort.Sort.Logic.Merge;
using LargeSort.Sort.Logic.Sorting;
using Serilog;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly string _tempFolder;
        private readonly ISortingAlgorithm _sortingAlgorithm;
        private readonly ILogger _logger;
        private readonly ConveyorPreSorter _preSorter;
        private readonly MultiWayFilesMerge _filesMerge;

        public Sorter(string inputFile, string tempFolder, ISortingAlgorithm sortingAlgorithm, ILogger logger)
        {
            _inputFile = inputFile;
            _tempFolder = tempFolder;
            _sortingAlgorithm = sortingAlgorithm;
            _logger = logger;
            _preSorter = new ConveyorPreSorter(logger);
            _filesMerge = new MultiWayFilesMerge(logger);
        }

        public void Sort(int bathSize, IWriter outputWriter, int parallels)
        {
            _preSorter.PreSort(_inputFile,_tempFolder,_sortingAlgorithm,bathSize, parallels);
            _filesMerge.Merge(_tempFolder, outputWriter);
        }
    }
}
