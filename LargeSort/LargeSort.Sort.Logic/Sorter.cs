using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LargeSort.FileSystem;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly string _outputFile;

        public Sorter(string inputFile, string outputFile)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
        }

        public void Sort()
        {
            var tempDir = Directory.CreateDirectory(Path.Combine(_outputFile, Path.GetRandomFileName()));

            var reader = new BatchFileReader(_inputFile);
            List<string> readed;
            while ((readed = reader.ReadNextBath(512*1024*1024)).Count != 0)
            {
                Console.WriteLine("Сортировка");
                var copy = readed.ToArray();
                CSharpStringSort.Sedgewick.InPlaceSort(copy, 0, copy.Length, 0);
                //readed.Sort();
                var writer = new FileStreamWriter(
                    Path.Combine(tempDir.FullName, Path.GetRandomFileName()),
                    FileMode.Create);

                Console.WriteLine("Запись");
                foreach (var line in readed)
                {
                    writer.Append(Encoding.UTF8.GetBytes(line));
                }

                writer.Flush();
                writer.Dispose();

                Console.WriteLine("Чтение");
            }
            //tempDir.Delete(true);
        }
    }
}
