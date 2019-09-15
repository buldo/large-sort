using System;
using System.Collections.Generic;
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

        public Sorter(string inputFile, string tempFolder, ISortingAlgorithm sortingAlgorithm, ILogger logger)
        {
            _inputFile = inputFile;
            _tempFolder = tempFolder;
            _sortingAlgorithm = sortingAlgorithm;
            _logger = logger;
        }

        public void Sort(int bathSize, IWriter outputWriter)
        {
            PreSort(bathSize);

            MultiWayFilesMerge.Merge(_tempFolder, outputWriter);
        }

        private void PreSort(int bathSize)
        {
            _logger.Information("Pre sorting");

            using (var reader = new BatchFileReader(_inputFile))
            {
                List<string> readed;
                while ((readed = ReadNext(reader, bathSize, _logger)).Count != 0)
                {
                    _logger.Debug("Sorting");
                    _sortingAlgorithm.Sort(readed);

                    using var writer = new FileStreamWriter(Path.Combine(_tempFolder, Path.GetRandomFileName()),FileMode.Create);
                    _logger.Debug("Writing");
                    foreach (var line in readed)
                    {
                        writer.Append(Encoding.UTF8.GetBytes(line));
                        writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
                    }

                    readed = null;
                    GC.Collect();
                }
            }
        }

        private static List<string> ReadNext(BatchFileReader batchFileReader, int size, ILogger logger)
        {
            logger.Debug("Reading next batch");
            var strings = batchFileReader.ReadNextBath(size);
            logger.Debug($"Read {strings.Count} lines");

            return strings;
        }
    }
}
