using System;
using System.Diagnostics;
using CommandLine;
using LargeSort.Sort.Logic;
using Serilog;

namespace LargeSort.Sort
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(Sort);
        }

        private static void Sort(Options options)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var watch = Stopwatch.StartNew();
            var sorter = new Sorter(options.InputFile, options.OutputFile, SortingAlgorithms.Simple, logger);
            sorter.Sort(16777216);
            watch.Stop();
            Console.WriteLine($"Отсортировано за {watch.Elapsed.ToString()}");
        }
    }
}
