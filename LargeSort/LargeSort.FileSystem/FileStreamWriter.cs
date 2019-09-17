using System;
using System.IO;

namespace LargeSort.FileSystem
{
    public class FileStreamWriter : IWriter
    {
        private readonly StreamWriter _streamWriter;

        public FileStreamWriter(string path, bool append)
        {
            _streamWriter = new StreamWriter(path, append);
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
        }

        public void Dispose()
        {
            Flush();
            _streamWriter.Dispose();
        }
    }
}
