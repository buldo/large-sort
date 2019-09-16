using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LargeSort.FileSystem;
using LargeSort.Sort.Logic.Sorting;
using Serilog;

namespace LargeSort.Sort.Logic
{
    class ConveyorPreSorter
    {
        private static readonly byte[] NewLineBytes = Encoding.UTF8.GetBytes(Environment.NewLine);
        private readonly ILogger _logger;

        public ConveyorPreSorter(ILogger logger)
        {
            _logger = logger;
        }

        public void PreSort(string inputFile, string tempFolder, ISortingAlgorithm sortingAlgorithm, int bathSize, int parallels)
        {
            _logger.Information("Pre sorting");
            var watch = Stopwatch.StartNew();
            using (var reader = new BatchFileReader(inputFile))
            {
                var semaphore = new SemaphoreSlim(parallels, parallels);

                var tasks = new List<Task>();

                List<CompositeString> readed;
                while ((readed = ReadNext(reader, bathSize, _logger)).Count != 0)
                {
                    semaphore.Wait();
                    var clone = readed.ToList();
                    tasks.Add(
                        Task.Factory.StartNew(
                            () => StartSortAndWrite(
                                sortingAlgorithm,
                                clone,
                                tempFolder,
                                semaphore,
                                _logger),
                            TaskCreationOptions.LongRunning));
                }

                Task.WaitAll(tasks.ToArray());
            }
            watch.Stop();
            _logger.Information($"Pre sorting ended in {watch.Elapsed.ToString()}");
        }

        private static List<CompositeString> ReadNext(BatchFileReader readerWriter, int size, ILogger logger)
        {
            logger.Debug("Reading next batch");
            var strings = readerWriter.ReadNextBath(size);
            logger.Debug($"Read {strings.Count} lines");

            return strings;
        }

        private static void StartSortAndWrite(
            ISortingAlgorithm sortingAlgorithm,
            List<CompositeString> toSort,
            string tempFolder,
            SemaphoreSlim semaphore,
            ILogger logger)
        {
            logger.Debug("Sorting");
            sortingAlgorithm.Sort(toSort);

            using (var writer = new FileStreamWriter(Path.Combine(tempFolder, Path.GetRandomFileName()), FileMode.Create))
            {
                logger.Debug("Writing");
                foreach (var line in toSort)
                {
                    writer.Append(Encoding.UTF8.GetBytes(line.Original));
                    writer.Append(NewLineBytes);
                }

                writer.Flush();
            }

            semaphore.Release();
        }
    }
}
