using System;
using System.IO;

namespace LargeSort.FileSystem
{
    public class FileStreamWriter : IWriter, IDisposable
    {
        private readonly FileStream _stream;

        public FileStreamWriter(string path)
        {
            _stream = new FileStream(path, FileMode.Append);
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
            _stream.Dispose();
        }
    }
}
