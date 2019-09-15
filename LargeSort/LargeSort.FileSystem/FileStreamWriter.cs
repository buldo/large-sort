using System;
using System.IO;

namespace LargeSort.FileSystem
{
    public class FileStreamWriter : IWriter
    {
        private readonly FileStream _stream;

        public FileStreamWriter(string path, FileMode mode)
        {
            _stream = new FileStream(path, mode);
        }

        public void Append(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }

        public void Append(ReadOnlySpan<byte> data)
        {
            _stream.Write(data);
        }

        public void Flush()
        {
            _stream.Flush(true);
        }

        public void Dispose()
        {
            _stream.Flush(true);
            _stream.Dispose();
        }
    }
}
