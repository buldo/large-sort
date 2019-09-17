using System;

namespace LargeSort.Generator.Logic
{
    internal class DataProvider
    {
        private readonly Random _random = new Random();
        private readonly WordsProvider _wordsProvider;

        public DataProvider(string dictionaryPath)
        {
            _wordsProvider = new WordsProvider(_random, dictionaryPath);
        }

        public string GetNextNumber(int max)
        {
            return _random.Next(max).ToString();
        }

        public string GetNextWord() => _wordsProvider.GetNextWord();
    }
}
