using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Serilog;

namespace LargeSort.Sort.Logic.Merge
{
    public class FileInMerge : IComparable<FileInMerge>, IDisposable
    {
        private readonly StreamReader _reader;

        public FileInMerge(string fileName, ILogger logger)
        {
            FileName = fileName;
            while (_reader == null)
            {
                try
                {
                    _reader = new StreamReader(fileName, Encoding.UTF8);
                }
                catch (Exception e)
                {
                    logger.Error(e, "Not able to open file");
                    Thread.Sleep(50);
                }
            }
        }

        public string FileName { get; }

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
            var compareResult = string.Compare(CurrentValue, other.CurrentValue, StringComparison.Ordinal);
            if (compareResult == 0)
            {
                compareResult = string.Compare(FileName, other.FileName, StringComparison.Ordinal);
            }

            return compareResult;
        }
    }
}
