using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LargeSort.FileSystem;

namespace LargeSort.Sort.Logic.Merge
{
    internal static class MultiWayFilesMerge
    {
        public static void Merge(string inputFolder, IWriter writer)
        {
            var files = Directory.GetFiles(inputFolder);
            var filesList = new List<FileInMerge>(files.Length);
            foreach (var file in files)
            {
                var fileInMerge = new FileInMerge(file);
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
                writer.Append(Encoding.UTF8.GetBytes(min.CurrentValue));
                writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
                if (min.ReadNext())
                {
                    sortedFiles.Add(min);
                }
            }
        }
    }
}
