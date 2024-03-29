﻿using System;
using System.Diagnostics;
using System.IO;
using CommandLine;

namespace LargeSort.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(GenerateData);
        }

        private static void GenerateData(Options options)
        {
            var writer = new StreamWriter(options.OutputFile, false);
            var generator = new Logic.Generator(options.DictionaryFile, writer);
            var watch = Stopwatch.StartNew();
            generator.Generate(options.SizeInGb * (long)1073741824);
            generator.Dispose();
            watch.Stop();
            Console.WriteLine($"Сгенерировано за {watch.Elapsed.ToString()}");
        }
    }
}
