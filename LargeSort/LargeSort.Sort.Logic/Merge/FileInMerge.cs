using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LargeSort.Sort.Logic.Merge
{
    public class FileInMerge : IComparable<FileInMerge>, IDisposable
    {
        private readonly StreamReader _reader;

        public FileInMerge(string fileName)
        {
            _reader = new StreamReader(fileName);
        }

        public string CurrentValue { get; private set; }

        public bool ReadNext()
        {
            CurrentValue = _reader.ReadLine();
            return !string.IsNullOrWhiteSpace(CurrentValue);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public int CompareTo(FileInMerge other)
        {
            return string.Compare(CurrentValue, other.CurrentValue, StringComparison.Ordinal);
        }
    }
}
