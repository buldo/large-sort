using System;
using System.IO;
using System.Text;

namespace LargeSort.Generator.Logic
{
    internal class WordsProvider
    {
        private readonly Random _random;
        private readonly string[] _seedWords;

        public WordsProvider(Random random, string dictionaryPath)
        {
            _random = random;
            _seedWords = File.ReadAllLines(dictionaryPath);
        }

        public ReadOnlySpan<byte> GetNextWordBytes()
        {
            var wordId = _random.Next(0, _seedWords.Length);
            return Encoding.UTF8.GetBytes(_seedWords[wordId]);
        }

        public string GetNextWord()
        {
            var wordId = _random.Next(0, _seedWords.Length);
            return _seedWords[wordId];
        }

    }
}