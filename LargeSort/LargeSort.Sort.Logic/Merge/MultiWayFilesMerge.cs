using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                if (!sortedFiles.Remove(min))
                {

                }
                writer.Append(Encoding.UTF8.GetBytes(min.CurrentValue));
                writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
                if (min.ReadNext())
                {
                    if (!sortedFiles.Add(min))
                    {

                    }
                }
            }

            //while (filesList.Count != 0)
            //{
            //    filesList.Sort();
            //    writer.Append(Encoding.UTF8.GetBytes(filesList[0].CurrentValue));
            //    writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
            //    if (!filesList[0].ReadNext())
            //    {
            //        filesList.RemoveAt(0);
            //    }
            //}

            watch.Stop();
            _logger.Information($"Merge ended in {watch.Elapsed.ToString()}");
        }
    }
}
