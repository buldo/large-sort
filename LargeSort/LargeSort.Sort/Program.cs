﻿using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using LargeSort.FileSystem;
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
            var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(options.OutputFile), Path.GetRandomFileName()));
            var sorter = new Sorter(options.InputFile, tempDir.FullName, SortingAlgorithms.Simple, logger);
            using (var writer = new FileStreamWriter(options.OutputFile, FileMode.Create))
            {
                sorter.Sort(256 * 1048576, writer);
            }


            watch.Stop();
            Console.WriteLine($"Отсортировано за {watch.Elapsed.ToString()}");
        }
    }
}
