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
        private static readonly object IoLock = new object();
        private readonly ILogger _logger;
        private readonly ManualResetEventSlim _readEvent = new ManualResetEventSlim(true);

        public ConveyorPreSorter(ILogger logger)
        {
            _logger = logger;
        }

        public void PreSort(string inputFile, string tempFolder, ISortingAlgorithm sortingAlgorithm, int bathSize, int parallels)
        {
            _logger.Information("Pre sorting");
            var watch = Stopwatch.StartNew();

            using (var reader = new BatchFileReader(inputFile, _readEvent))
            {
                //var semaphore = new SemaphoreSlim(parallels, parallels);

                var tasks = new List<Task>();

                List<CompositeString> readed;
                while ((readed = ReadNext(reader, bathSize, _logger)).Count != 0)
                {
                    //  semaphore.Wait();
                    //  _logger.Debug($"{parallels-semaphore.CurrentCount} потоков работает");

                    using (var writer = new FileStreamWriter(Path.Combine(tempFolder, Path.GetRandomFileName()), false))
                    {
                        sortingAlgorithm.Sort(readed);
                        foreach (var s in readed)
                        {
                            writer.AppendLine(s.Original);
                        }
                    }
                   // continue;
                    //    var clone = readed.ToList();

                    //tasks.Add(
                    //    Task.Factory.StartNew(
                    //        () => StartSortAndWrite(
                    //            sortingAlgorithm,
                    //            clone,
                    //            tempFolder,
                    //            semaphore,
                    //            _readEvent,
                    //            _logger),
                    //        TaskCreationOptions.LongRunning));
                }

                Task.WaitAll(tasks.ToArray());
            }
            watch.Stop();
            _logger.Information($"Pre sorting ended in {watch.Elapsed.ToString()}");
        }

        private static List<CompositeString> ReadNext(BatchFileReader readerWriter, int size, ILogger logger)
        {
            logger.Debug("Reading next batch");
            List<CompositeString> strings;
            //lock (IoLock)
            {
                strings = readerWriter.ReadNextBath(size);
            }
            logger.Debug($"Read {strings.Count} lines");

            return strings;
        }

        private static void StartSortAndWrite(
            ISortingAlgorithm sortingAlgorithm,
            List<CompositeString> toSort,
            string tempFolder,
            SemaphoreSlim semaphore,
            ManualResetEventSlim readEvent,
            ILogger logger)
        {
            logger.Debug("Sorting");
            sortingAlgorithm.Sort(toSort);

            using (var writer = new FileStreamWriter(Path.Combine(tempFolder, Path.GetRandomFileName()), false))
            {
                logger.Debug("Writing");

                //lock(IoLock)
                {
                    //readEvent.Reset();
                    foreach (var line in toSort)
                    {
                        writer.AppendLine(line.Original);
                    }

                   // readEvent.Set();
                }
            }

            semaphore.Release();
        }
    }
}
