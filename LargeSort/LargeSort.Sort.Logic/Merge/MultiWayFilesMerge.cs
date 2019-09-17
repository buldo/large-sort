using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using LargeSort.FileSystem;
using Serilog;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MultiWayFilesMerge
    {
        private readonly ILogger _logger;

        public MultiWayFilesMerge(ILogger logger)
        {
            _logger = logger;
        }

        public void Merge(string inputFolder, IWriter writer)
        {
            _logger.Information("Merge started");

            var files = Directory.GetFiles(inputFolder);
            _logger.Information($"Finds {files.Length} temp files");
            var watch = Stopwatch.StartNew();
            var filesList = new List<FileInMerge>(files.Length);
            foreach (var file in files)
            {
                var fileInMerge = new FileInMerge(file, _logger);
                if (fileInMerge.ReadNext())
                {
                    filesList.Add(fileInMerge);
                }
            }

            var sortedFiles = new SortedSet<FileInMerge>(filesList);

            while (sortedFiles.Count != 0)
            {
                var min = sortedFiles.Min;
                sortedFiles.Remove(min);
                writer.AppendLine(min.CurrentValue.Original);
                if (min.ReadNext())
                {
                    sortedFiles.Add(min);
                }
            }

            writer.Flush();

            watch.Stop();
            _logger.Information($"Merge ended in {watch.Elapsed.ToString()}");
        }
    }
}
