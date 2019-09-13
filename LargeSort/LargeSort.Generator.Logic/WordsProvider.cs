using System;
using System.IO;
using System.Text;

namespace LargeSort.Generator.Logic
{
    internal class WordsProvider
    {
        private readonly Random _random;
        private readonly string[] _seedWords;
        private readonly Encoder _encoder = Encoding.UTF8.GetEncoder();
        private readonly byte[] _nextWordBuffer = new byte[2048];

        public WordsProvider(Random random, string dictionaryPath)
        {
            _random = random;
            _seedWords = File.ReadAllLines(dictionaryPath);
        }

        public ReadOnlySpan<byte> GetNextWordBytes()
        {
            var wordId = _random.Next(0, _seedWords.Length);
            var written = _encoder.GetBytes(_seedWords[wordId].AsSpan(), _nextWordBuffer.AsSpan(), true);
            return _nextWordBuffer.AsSpan(0, written);
        }
    }
}