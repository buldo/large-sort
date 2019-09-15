﻿using System;
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
            while (generated < size)
            {
                // TODO: Вынести генерацию в отдельный поток
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
