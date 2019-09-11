using System;
using System.Diagnostics;
using LargeSort.FileSystem;

namespace LargeSort.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            long written = 0;
            long target = (long)4 * 1024 * 1024 * 1024;
            var writer = new FileStreamWriter(@"D:\testFolder\out");
            var watch = Stopwatch.StartNew();
            //while (written < target)
            //{
            //    writer.Append(Guid.NewGuid().ToByteArray());
            //    written += 16;
            //}
            var dataProvider = new DataProvider();
            writer.Append(dataProvider.GetNextNumberBytes());
            writer.Append(dataProvider.DotSpaceBytes);
            writer.Append(dataProvider.GetNextWordBytes());
            writer.Append(dataProvider.LineEndBytes);
            writer.Flush();
            writer.Dispose();
            watch.Stop();

            Console.WriteLine(watch.Elapsed.ToString());
        }
    }
}
