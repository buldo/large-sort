using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LargeSort.FileSystem;
using LargeSort.Sort.Logic.Sorting;
using Serilog;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly string _outputFile;
        private readonly ISortingAlgorithm _sortingAlgorithm;
        private readonly ILogger _logger;

        public Sorter(string inputFile, string outputFile, ISortingAlgorithm sortingAlgorithm, ILogger logger)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
            _sortingAlgorithm = sortingAlgorithm;
            _logger = logger;
        }

        public void Sort(int bathSize, DirectoryInfo tempDir = null, bool removeTempDir = true)
        {
            tempDir ??= Directory.CreateDirectory(Path.Combine(_outputFile, Path.GetRandomFileName()));

            PreSort(tempDir);

            if (removeTempDir)
            {
                tempDir.Delete(true);
            }
        }

        private void PreSort(DirectoryInfo tempDir)
        {
            _logger.Information("Pre sorting");

            using (var reader = new BatchFileReader(_inputFile))
            {
                List<string> readed;
                while ((readed = ReadNext(reader)).Count != 0)
                {
                    _logger.Debug("Sorting");
                    _sortingAlgorithm.Sort(readed);

                    using var writer = new FileStreamWriter(Path.Combine(tempDir.FullName, Path.GetRandomFileName()),FileMode.Create);
                    _logger.Debug("Writing");
                    foreach (var line in readed)
                    {
                        writer.Append(Encoding.UTF8.GetBytes(line));
                        writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
                    }
                }
            }

            List<string> ReadNext(BatchFileReader batchFileReader)
            {
                _logger.Debug("Reading next batch");
                var strings = batchFileReader.ReadNextBath((long) 2048 * 1024 * 1024);
                _logger.Debug($"Read {strings.Count} lines");

                return strings;
            }
        }
    }
}
