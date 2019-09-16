using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LargeSort.Sort.Logic
{
    internal class BatchFileReader : IDisposable
    {
        private readonly StreamReader _reader;

        private int _prevSize = 0;

        public BatchFileReader(string fileName)
        {
            _reader = new StreamReader(fileName, Encoding.UTF8, false, 256);
        }

        public List<CompositeString> ReadNextBath(long bytesInMemory)
        {
            var ret = new List<CompositeString>(_prevSize == 0 ? 10000 : _prevSize);
            string line;
            long reads = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                reads += line.Length * 4 + 18 + 4 + 8;
                ret.Add(new CompositeString(line));
                if (reads > bytesInMemory)
                {
                    break;
                }
            }

            _prevSize = ret.Count;

            return ret;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
