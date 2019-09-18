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

        private static void Sort(Options obj)
        {
            var watch = Stopwatch.StartNew();
            var sorter = new Sorter(obj.InputFile);
            sorter.Sort(obj.OutputFile, obj.ThreadsCount);
            watch.Stop();
            Console.WriteLine($"Elapsed {watch.Elapsed.ToString()}");
        }
    }
}
