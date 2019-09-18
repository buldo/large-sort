using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic
{
    class SortingTask
    {
        private const int MaxCount = 6000000;
        private static readonly CompositeStringComparer Comparer = new CompositeStringComparer();
        private readonly StreamReader _reader;
        private readonly string[] _stringList = new string[MaxCount];
        private readonly CompositeString[] _list = new CompositeString[MaxCount];

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

                Array.Sort(_list, 0, Count, Comparer);
                return this;
            });
        }

        public void Write(string path, SemaphoreSlim semaphore)
        {
            using (var writer = new StreamWriter(path))
            {
                for (int i = 0; i < Count; i++)
                {
                    writer.WriteLine(_list[i].Original);
                }
            }

            semaphore.Release();
        }
    }
}