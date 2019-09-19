using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MultiFilesMerge
    {
        private static MergeSourceComparer _comparer = new MergeSourceComparer();
        private readonly string _inputFolder;

        public MultiFilesMerge(string inputFolder)
        {
            _inputFolder = inputFolder;
        }

        public void MultiLevelMerge(string outputFile)
        {
            MultiLevelMerge(outputFile, 0);
        }

        private void MultiLevelMerge(string outputFile, int level)
        {
            var sources = GetMergeSources(_inputFolder, level == 0 ? "*" : $"*_{level}");
            if (sources.Count == 2)
            {
                TwoWayMerge(sources[0], sources[1], outputFile);
                return;
            }

            if (sources.Count == 3)
            {
                Merge(sources.ToArray(), outputFile);
                return;
            }

            level++;
            int start = 0;
            if (sources.Count % 2 == 1)
            {
                Merge(new[] { sources[0], sources[1], sources[2] }, Path.Combine(_inputFolder, $"{Path.GetRandomFileName()}_{level}"));
                start+=3;
            }

            var tasks = new List<Task>();
            for (int i = start; i < sources.Count; i += 2)
            {
                var sources1 = sources;
                var i1 = i;
                tasks.Add(Task.Run(() => TwoWayMerge(sources1[i1], sources1[i1 + 1],
                    Path.Combine(_inputFolder, $"{Path.GetRandomFileName()}_{level}"))));
            }

            Task.WaitAll(tasks.ToArray());
            sources = null;
            MultiLevelMerge(outputFile, level);
        }


        private void Merge(MergeSource[] sources, string outputFile)
        {
            var set = new SortedSet<MergeSource>(sources, new MergeSourceComparer());

            using (var writer = new StreamWriter(outputFile))
            {
                const int bufferSize = 91750400;
                var buffer = new string[bufferSize];
                var cnt = 0;

                while (set.Count != 0)
                {
                    var min = set.Min;
                    set.Remove(min);
                    buffer[cnt] = min.Current.Original;
                    cnt++;
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

        private void TwoWayMerge(MergeSource s1, MergeSource s2, string outputFile)
        {
            using (var writer = new StreamWriter(outputFile))
            {
                while (!s1.IsEnded && !s2.IsEnded)
                {
                    var compareResult = _comparer.Compare(s1, s2);

                    if (compareResult == 0)
                    {
                        writer.WriteLine(s1.Current.Original);
                        writer.WriteLine(s2.Current.Original);
                        s1.Next();
                        s2.Next();
                    }
                    else if(compareResult < 0)
                    {
                        writer.WriteLine(s1.Current.Original);
                        s1.Next();
                    }
                    else if (compareResult > 0)
                    {
                        writer.WriteLine(s2.Current.Original);
                        s2.Next();
                    }
                }

                while (!s1.IsEnded)
                {
                    writer.WriteLine(s1.Current.Original);
                    s1.Next();
                }

                while (!s2.IsEnded)
                {
                    writer.WriteLine(s2.Current.Original);
                    s2.Next();
                }
            }
        }

        private List<MergeSource> GetMergeSources(string inputFolder, string searchPattern)
        {
            var allFiles = Directory.GetFiles(inputFolder, searchPattern);
            var list = new List<MergeSource>(allFiles.Length);
            foreach (var file in allFiles)
            {
                var source = new MergeSource(file);
                if (source.Next())
                {
                    list.Add(source);
                }
            }

            return list;
        }
    }
}
