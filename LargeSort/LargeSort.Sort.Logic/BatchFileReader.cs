using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LargeSort.Sort.Logic
{
    internal class BatchFileReader : IDisposable
    {
        private readonly StreamReader _reader;

        public BatchFileReader(string fileName)
        {
            _reader = new StreamReader(fileName, Encoding.UTF8);
        }

        public List<string> ReadNextBath(long bytesInMemory)
        {
            var ret = new List<string>(4096);
            string line;
            long reads = 0;
            while ((line = _reader.ReadLine()) != null && reads < bytesInMemory)
            {
                reads += Encoding.Default.GetByteCount(line) + 18;
                ret.Add(line);
            }

            return ret;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
