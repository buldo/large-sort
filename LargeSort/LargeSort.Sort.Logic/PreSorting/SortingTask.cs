using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HPCsharp;

namespace LargeSort.Sort.Logic.PreSorting
{
    class SortingTask
    {
        private const int MaxCount = 65536;
        private static readonly CompositeStringComparer Comparer = new CompositeStringComparer();
        private readonly StreamReader _reader;
        private readonly string[] _stringList = new string[MaxCount];
        private CompositeString[] _list = new CompositeString[MaxCount];

        public SortingTask(StreamReader reader)
        {
            _reader = reader;
            for (int i = 0; i < MaxCount; i++)
            {
                _list[i] = new CompositeString();
            }
        }

        public int Count { get; private set; }

        public bool Read()
        {
            string line;
            Count = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                _stringList[Count] = line;
                Count++;
                if (Count >= MaxCount)
                {
                    break;
                }
            }

            return Count != 0;
        }

        public Task<SortingTask> Sort()
        {
            return Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    _list[i].Init(_stringList[i]);
                }

                _list = _list.SortMergePar(Comparer);

                Array.Sort(_list, 0, Count, Comparer);
                return this;
            });
        }

        public void Write(string path, SemaphoreSlim semaphore)
        {
            using (var writer = new BufferedStream(new FileStream(path, FileMode.Create), 26 * 1048576) )
            {
                var end = Encoding.UTF8.GetBytes(Environment.NewLine);
                for (int i = 0; i < Count; i++)
                {
                    writer.Write(Encoding.UTF8.GetBytes(_list[i].Original));
                    writer.Write(end);
                }
            }

            semaphore.Release();
        }
    }
}