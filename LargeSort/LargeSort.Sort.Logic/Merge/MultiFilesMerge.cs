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
                //var list = new List<string>();
                //int i = 0;
                while (set.Count != 0)
                {
                    var min = set.Min;
                    set.Remove(min);
                    writer.WriteLine(min.Current.Original);
                    //i++;
                    //list.Add($"{i}_{min.Current.Original}");
                    if (min.Next())
                    {
                        set.Add(min);
                    }
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
