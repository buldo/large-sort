using System;
using System.IO;

namespace LargeSort.FileSystem
{
    public class FileStreamWriter : IWriter
    {
        private readonly FileStream _stream;
        private readonly StreamWriter _streamWriter;

        public FileStreamWriter(string path, FileMode mode)
        {
            _stream = new FileStream(path, mode, FileAccess.Write, FileShare.None, 8192);
            _streamWriter = new StreamWriter(_stream);
        }

        public void Append(byte[] data)
        {
            _streamWriter.Write(data);
        }

        public void Append(ReadOnlySpan<byte> data)
        {
            _streamWriter.Write(data.ToArray());
        }

        public void AppendLine(string data)
        {
            _streamWriter.WriteLine(data);
        }

        public void Flush()
        {
            _streamWriter.Flush();
            _stream.Flush(true);
        }

        public void Dispose()
        {
            Flush();
            _streamWriter.Dispose();
            _stream.Dispose();
        }
    }
}
