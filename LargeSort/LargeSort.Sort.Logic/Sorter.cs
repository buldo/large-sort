using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LargeSort.FileSystem;
using LargeSort.Sort.Logic.Sorting;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly string _outputFile;
        private readonly ISortingAlgorithm _sortingAlgorithm;

        public Sorter(string inputFile, string outputFile, ISortingAlgorithm sortingAlgorithm)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
            _sortingAlgorithm = sortingAlgorithm;
        }

        public void Sort(DirectoryInfo tempDir = null)
        {
            tempDir ??= Directory.CreateDirectory(Path.Combine(_outputFile, Path.GetRandomFileName()));

            var reader = new BatchFileReader(_inputFile);
            List<string> readed;
            while ((readed = reader.ReadNextBath((long)2048*1024*1024)).Count != 0)
            {
                Console.WriteLine("Сортировка");
                _sortingAlgorithm.Sort(readed);

                var writer = new FileStreamWriter(
                    Path.Combine(tempDir.FullName, Path.GetRandomFileName()),
                    FileMode.Create);

                Console.WriteLine("Запись");
                foreach (var line in readed)
                {
                    writer.Append(Encoding.UTF8.GetBytes(line));
                    writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
                }

                writer.Dispose();

                Console.WriteLine("Чтение");
            }
            //tempDir.Delete(true);
        }
    }
}
