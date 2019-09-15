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

        public List<string> ReadNextBath(long bytesInMemory)
        {
            var ret = new List<string>(_prevSize == 0 ? 10000 : _prevSize);
            string line;
            long reads = 0;
            while ((line = _reader.ReadLine()) != null && reads < bytesInMemory)
            {
                reads += Encoding.Default.GetByteCount(line) + 18;
                ret.Add(line);
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
