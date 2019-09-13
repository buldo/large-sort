using System;
using System.Collections.Generic;
using System.Text;
using LargeSort.FileSystem;

namespace LargeSort.Generator.Logic
{
    public class Generator : IDisposable
    {
        private readonly IWriter _writer;
        private readonly DataProvider _dataProvider;

        /// <param name="dictionaryPath">Путь к словарю</param>
        /// <param name="writer">Через что будем писать</param>
        public Generator(string dictionaryPath, IWriter writer)
        {
            _writer = writer;
            _dataProvider = new DataProvider(dictionaryPath);
        }

        /// <summary>
        /// Сгенерировать файл.
        /// </summary>
        /// <param name="sizeInGb">Примерный размер файла в GB</param>
        public void Generate(long sizeInGb)
        {
            long generated = 0;
            long toBeGenerated = sizeInGb * 1024 * 1024 * 1024;
            while (generated < toBeGenerated)
            {
                var nextNumberBytes = _dataProvider.GetNextNumberBytes();
                _writer.Append(nextNumberBytes);
                generated += nextNumberBytes.Length;

                _writer.Append( _dataProvider.DotSpaceBytes);
                generated += _dataProvider.DotSpaceBytes.Length;

                var nextNextWordBytes = _dataProvider.GetNextWordBytes();
                _writer.Append(nextNextWordBytes);
                generated += nextNextWordBytes.Length;

                _writer.Append(_dataProvider.LineEndBytes);
                generated += _dataProvider.LineEndBytes.Length;
            }

            _writer.Flush();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
