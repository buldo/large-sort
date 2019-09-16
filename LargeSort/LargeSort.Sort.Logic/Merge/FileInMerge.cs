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

        public CompositeString CurrentValue { get; private set; }

        public static bool operator <(FileInMerge first, FileInMerge second)
        {

            return Compare(first, second) < 0;

        }

        public static bool operator >(FileInMerge first, FileInMerge second)
        {

            return Compare(first, second) > 0;

        }

        public bool ReadNext()
        {
            var line = _reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                CurrentValue = null;
                return false;
            }

            CurrentValue = new CompositeString(line);
            return true;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public int CompareTo(FileInMerge other)
        {
            return Compare(this, other);
        }

        private static int Compare(FileInMerge first, FileInMerge second)
        {
            var compareResult = first.CurrentValue.CompareTo(second.CurrentValue);
            if (compareResult == 0)
            {
                compareResult = string.Compare(first.FileName, second.FileName, StringComparison.Ordinal);
            }

            return compareResult;
        }
    }
}
