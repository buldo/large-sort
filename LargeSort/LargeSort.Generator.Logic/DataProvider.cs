using System;
using System.Buffers.Text;
using System.Text;

namespace LargeSort.Generator.Logic
{
    internal class DataProvider
    {

        private readonly Random _random = new Random();
        private readonly byte[] _nextNumberBuffer = new byte[2048];
        private readonly WordsProvider _wordsProvider;

        public DataProvider(string dictionaryPath)
        {
            _wordsProvider = new WordsProvider(_random, dictionaryPath);
        }

        public ReadOnlySpan<byte> GetNextNumberBytes()
        {
            var number = _random.Next();
            Utf8Formatter.TryFormat(number, _nextNumberBuffer.AsSpan(), out var written);
            return _nextNumberBuffer.AsSpan(0, written);
        }

        public byte[] DotSpaceBytes { get; } = Encoding.UTF8.GetBytes(new[] { '.', ' ' });

        public byte[] LineEndBytes { get; } = Encoding.UTF8.GetBytes(Environment.NewLine.ToCharArray());

        public ReadOnlySpan<byte> GetNextWordBytes() => _wordsProvider.GetNextWordBytes();
    }
}
