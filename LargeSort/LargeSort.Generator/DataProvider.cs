using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LargeSort.Generator
{
    class DataProvider
    {

        private readonly Random _random = new Random();
        private readonly byte[] _nextNumberBuffer = new byte[2048];
        private readonly WordsProvider _wordsProvider;

        public DataProvider()
        {
            _wordsProvider = new WordsProvider(_random);
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


        private class WordsProvider
        {
            private readonly Random _random;
            private readonly string[] _seedWords;
            private readonly Encoder _encoder = Encoding.UTF8.GetEncoder();
            private readonly byte[] _nextWordBuffer = new byte[2048];

            public WordsProvider(Random random)
            {
                _random = random;
                _seedWords = File.ReadAllLines("dictionary.txt");
            }

            public ReadOnlySpan<byte> GetNextWordBytes()
            {
                var wordId = _random.Next(0, _seedWords.Length);
                var written = _encoder.GetBytes(_seedWords[wordId].AsSpan(), _nextWordBuffer.AsSpan(), true);
                return _nextWordBuffer.AsSpan(0, written);
            }
        }
    }
}
