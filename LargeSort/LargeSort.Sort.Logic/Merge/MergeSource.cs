using System;
using System.IO;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MergeSource
    {
        private readonly StreamReader _stream;

        public MergeSource(string fileName)
        {
            FileName = fileName;
            while (true)
            {
                try
                {
                    _stream = new StreamReader(fileName);
                    break;
                }
                catch
                {
                }
            }
        }
        public string FileName { get; }
        public CompositeString Current { get; } = new CompositeString();
        private int readed;
        public bool Next()
        {
            var line = _stream.ReadLine();
            if (line != null)
            {
                readed++;
                Current.Init(line);
                return true;
            }
            else
            {

            }

            return false;
        }
    }
}
