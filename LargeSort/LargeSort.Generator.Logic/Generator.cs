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
        /// <param name="size">Примерный размер файла в байтах</param>
        public void Generate(long size)
        {
            long generated = 0;
            string toWrite = null;
            while (generated < size)
            {
                //var toWrite = $"{_dataProvider.GetNextNumber()}. {_dataProvider.GetNextWord()}{Environment.NewLine}{_dataProvider.GetNextNumber()}. {_dataProvider.GetNextWord()}{Environment.NewLine}{_dataProvider.GetNextNumber()}. {_dataProvider.GetNextWord()}{Environment.NewLine}{_dataProvider.GetNextNumber()}. {_dataProvider.GetNextWord()}";
                toWrite = $"{_dataProvider.GetNextNumber(100)}. {_dataProvider.GetNextWord()}";
                generated += Encoding.UTF8.GetByteCount(toWrite);
                _writer.AppendLine(toWrite);
            }
            _writer.AppendLine(toWrite);

            _writer.Flush();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
