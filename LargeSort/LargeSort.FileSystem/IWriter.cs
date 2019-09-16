using System;

namespace LargeSort.FileSystem
{
    public interface IWriter : IDisposable
    {
        void Append(byte[] data);

        void Flush();
        void Append(ReadOnlySpan<byte> data);
        void AppendLine(string data);
    }
}
