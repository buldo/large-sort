using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace LargeSort.FileSystem.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<WriterBenchmark>();
        }
    }

    [SimpleJob(RunStrategy.ColdStart, launchCount: 1)] // Запись на диск сама по себе медленная. Прогрев не так уж и важен
    public class WriterBenchmark
    {
        private const string Path = @"D:\testFolder\out"; // Тестирую локально на своём HDD
        private readonly byte[] _data1 = new byte[2147483591]; // в .net нельзя сделать больше :(
        private readonly byte[] _data2 = new byte[2147483591]; //
        private FileStreamWriter _fileStreamWriter;

        public WriterBenchmark()
        {
            var random = new Random();
            random.NextBytes(_data1);
            random.NextBytes(_data2);
        }

        #region Stream

        [IterationCleanup(Target = nameof(StreamWriter))]
        public void IterationCleanup()
        {
            _fileStreamWriter?.Dispose();
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        [IterationSetup(Target = nameof(StreamWriter))]
        public void IterationSetup()
        {
            _fileStreamWriter = new FileStreamWriter(Path, FileMode.Create);
        }

        [Benchmark]
        public void StreamWriter()
        {
            _fileStreamWriter.Append(_data1);
            _fileStreamWriter.Append(_data2);
            _fileStreamWriter.Flush();
        }

        #endregion
    }
}
