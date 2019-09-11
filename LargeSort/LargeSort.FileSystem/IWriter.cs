using System;

namespace LargeSort.FileSystem
{
    public interface IWriter
    {
        void Append(byte[] data);

        void Flush();
    }
}
