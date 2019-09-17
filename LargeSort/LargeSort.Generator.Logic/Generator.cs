using System;
using System.IO;
using System.Text;

namespace LargeSort.Generator.Logic
{
    public class Generator : IDisposable
    {
        private readonly StreamWriter _writer;
        private readonly DataProvider _dataProvider;

        /// <param name="dictionaryPath">Путь к словарю</param>
        /// <param name="writer">Через что будем писать</param>
        public Generator(string dictionaryPath, StreamWriter writer)
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
                toWrite = $"{_dataProvider.GetNextNumber(100)}. {_dataProvider.GetNextWord()}";
                generated += Encoding.UTF8.GetByteCount(toWrite) +2; // минус пара байт на перенос строки
                _writer.WriteLine(toWrite);
            }
            _writer.WriteLine(toWrite);

            _writer.Flush();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
