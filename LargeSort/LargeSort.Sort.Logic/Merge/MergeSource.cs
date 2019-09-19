using System;
using System.IO;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MergeSource
    {
        private readonly StringsBuffer _buffer;

        public MergeSource(string fileName)
        {
            FileName = fileName;
            while (true)
            {
                try
                {
                    var reader = new StreamReader(fileName);
                    _buffer = new StringsBuffer(10000, reader);
                    break;
                }
                catch
                {
                }
            }
        }
        public string FileName { get; }
        public CompositeString Current { get; } = new CompositeString();

        public bool Next()
        {
            var line = _buffer.GetNext();
            if (line != null)
            {
                Current.Init(line);
                return true;
            }

            return false;
        }
    }

    internal class StringsBuffer
    {

        private readonly string[] _buffer;
        private readonly StreamReader _reader;
        private readonly int _size;

        private int _currentSize;
        private int _position;
        private bool _isEnded;

        public StringsBuffer(int bufferSize, StreamReader reader)
        {
            _size = bufferSize;
            _buffer = new string[bufferSize];
            _reader = reader;
        }

        public string GetNext()
        {
            if (_isEnded && _currentSize == _position)
            {
                return null;
            }


            if (_currentSize == 0 || _position == _currentSize)
            {
                Populate();
            }

            var current = _buffer[_position];
            _position++;
            return current;
        }

        private void Populate()
        {
            string line;
            _currentSize = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                _buffer[_currentSize] = line;
                _currentSize++;
                if (_currentSize == _size)
                {
                    break;
                }
            }

            if (_currentSize != _size)
            {
                _isEnded = true;
            }

            _position = 0;
        }
    }
}
