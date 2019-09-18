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

        private static void Sort(Options obj)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var watch = Stopwatch.StartNew();
            var sorter = new Sorter(obj.InputFile, logger);
            sorter.Sort(obj.OutputFile, obj.ThreadsCount);
            watch.Stop();
            Console.WriteLine($"Sorted in {watch.Elapsed.ToString()}");
        }
    }
}
