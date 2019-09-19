using System.Collections.Generic;
using System.IO;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MultiFilesMerge
    {
        public void Merge(string inputFolder, string outputFile)
        {
            var sources = GetMergeSources(inputFolder);
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

        private List<MergeSource> GetMergeSources(string inputFolder)
        {
            var allFiles = Directory.GetFiles(inputFolder);
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
