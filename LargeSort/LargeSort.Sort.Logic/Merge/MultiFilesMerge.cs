using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MultiFilesMerge
    {
        private const int mult = 8;
        private int _workersCount = Environment.ProcessorCount * mult;
        private static MergeSourceComparer _comparer = new MergeSourceComparer();
        private readonly string _inputFolder;

        public MultiFilesMerge(string inputFolder)
        {
            _inputFolder = inputFolder;
        }

        public void MultiLevelMerge(string outputFile)
        {
            //var filesToMerge = Directory.GetFiles(_inputFolder);
            //Merge(filesToMerge, outputFile);
            MultiLevelMerge(outputFile, 0);
        }

        private void MultiLevelMerge(string outputFile, int level)
        {
            var watch = Stopwatch.StartNew();
            var filesToMerge = Directory.GetFiles(_inputFolder, level == 0 ? "*" : $"*_{level}");

            if (filesToMerge.Length <= _workersCount*4)
            {
                Merge(filesToMerge, outputFile);
                return;
            }
            else
            {
                level++;

                var filesInBatch = (filesToMerge.Length / _workersCount) + 1;

                var tasks = new List<Task>(_workersCount / mult);

                for (int i = 0; i < _workersCount / mult; i++)
                {
                    var batch = new List<string>(filesInBatch);
                    for (int j = 0; j < filesInBatch; j++)
                    {
                        var index = filesInBatch * i + j;
                        if (index < filesToMerge.Length)
                        {
                            batch.Add(filesToMerge[index]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    var level1 = level;
                    tasks.Add(Task.Run(() =>
                    {
                        Merge(batch.ToArray(),
                            Path.Combine(_inputFolder, $"{Path.GetRandomFileName()}_{level1}"));
                    }));
                }

                Task.WaitAll(tasks.ToArray());
            }



            filesToMerge = null;
            watch.Stop();
            Console.WriteLine($"Level {level} merged for {watch.Elapsed.ToString()}");
            MultiLevelMerge(outputFile, level);
        }


        private void Merge(string[] sources, string outputFile)
        {
            if (sources.Length == 0)
            {
                return;
            }

            var unsorted = new List<MergeSource>();
            foreach (var source in sources)
            {
                var src = new MergeSource(source);
                src.Next();
                unsorted.Add(src);
            }

            var set = new SortedSet<MergeSource>(unsorted, _comparer);

            using (var writer = new StreamWriter(outputFile))
            {
                const int bufferSize = 91754;
                var buffer = new string[bufferSize];
                var cnt = 0;

                while (set.Count != 0)
                {
                    var min = set.Min;
                    set.Remove(min);
                    buffer[cnt] = min.Current.Original;
                    cnt++; // jnrk.xe pfgcbm
                    if (cnt == bufferSize)
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            writer.WriteLine(buffer[i]);
                        }

                        cnt = 0;
                    }

                    if (min.Next())
                    {
                        set.Add(min);
                    }
                }

                for (int i = 0; i < cnt; i++)
                {
                    writer.WriteLine(buffer[i]);
                }
            }
        }
    }
}
