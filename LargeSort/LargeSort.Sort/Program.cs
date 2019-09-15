using System;
using System.Diagnostics;
using CommandLine;
using LargeSort.Sort.Logic;

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
            var watch = Stopwatch.StartNew();
            var sorter = new Sorter(options.InputFile, options.OutputFile, SortingAlgorithms.Simple);
            sorter.Sort();
            watch.Stop();
            Console.WriteLine($"Отсортировано за {watch.Elapsed.ToString()}");
        }
    }
}
