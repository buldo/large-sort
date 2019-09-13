using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using LargeSort.FileSystem;

namespace LargeSort.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(GenerateData);
            //long written = 0;
            //long target = (long)4 * 1024 * 1024 * 1024;
            //var writer = new FileStreamWriter(@"D:\testFolder\out");
            //var watch = Stopwatch.StartNew();
            ////while (written < target)
            ////{
            ////    writer.Append(Guid.NewGuid().ToByteArray());
            ////    written += 16;
            ////}
            //var dataProvider = new DataProvider();

            //writer.Dispose();
            //watch.Stop();

            //Console.WriteLine(watch.Elapsed.ToString());
        }

        private static void GenerateData(Options options)
        {
            var writer = new FileStreamWriter(options.OutputFile, FileMode.Create);
            var generator = new Logic.Generator(options.DictionaryFile, writer);
            var watch = Stopwatch.StartNew();
            generator.Generate(options.SizeInGb);
            watch.Stop();
            Console.WriteLine($"Сгенерировано за {watch.Elapsed.ToString()}");
        }
    }
}
