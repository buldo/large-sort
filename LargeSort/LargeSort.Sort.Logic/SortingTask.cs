using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic
{
    class SortingTask
    {
        private static readonly CompositeStringComparer Comparer = new CompositeStringComparer();
        private StreamReader _reader;
        private List<string> _stringList = new List<string>(6000000);
        private List<CompositeString> _list = new List<CompositeString>(6000000);

        public SortingTask(StreamReader reader)
        {
            _reader = reader;
        }

        public bool Read()
        {
            string line;
            long cnt = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                cnt++;
                _stringList.Add(line);
                if (cnt >= 6000000)
                {
                    break;
                }
            }

            return _stringList.Count != 0;
        }

        public Task<SortingTask> Sort()
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var line in _stringList)
                {
                    _list.Add(new CompositeString(line));
                }

                _stringList = null;
                _list.Sort(Comparer);
                return this;
            });
        }

        public void Write(string path, SemaphoreSlim semaphore)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var line in _list)
                {
                    writer.WriteLine($"{line.Number}. {line.Word}");
                }
            }

            semaphore.Release();
            _list = null;
            _reader = null;
        }
    }
}