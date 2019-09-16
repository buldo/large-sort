using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace LargeSort.Sort.Logic
{
    internal class BlockedReaderWriter
    {
        private readonly BatchFileReader _fileReader;
        private readonly ILogger _logger;
        private readonly Task _readWriteTask;

        private readonly ConcurrentQueue<Func<List<string>>> _readQueue = new ConcurrentQueue<Func<List<string>>>();
        private readonly ConcurrentQueue<List<string>> _readResultQueue = new ConcurrentQueue<List<string>>();
        private readonly ManualResetEventSlim _readEvent = new ManualResetEventSlim(false);

        private Action _writeTask;
        private readonly ConcurrentQueue<Action> _writeQueue = new ConcurrentQueue<Action>();
        private readonly ManualResetEventSlim _writeEvent = new ManualResetEventSlim(false);

        private readonly object _readLock = new object();
        private readonly object _writeLock = new object();


        public BlockedReaderWriter(BatchFileReader fileReader, ILogger logger)
        {
            _fileReader = fileReader;
            _logger = logger;
            _readWriteTask = Task.Factory.StartNew(ReadWrite, TaskCreationOptions.LongRunning);
        }

        private void ReadWrite()
        {
            while (true)
            {
                if (_readQueue.IsEmpty && _writeTask == null)
                {
                    Thread.Sleep(500);
                }

                if (_writeTask != null && !_writeEvent.IsSet)
                {
                    var task = _writeTask;
                    _writeTask = null;
                    task();
                    _writeEvent.Set();
                }

                //if (_writeQueue.TryDequeue(out var action))
                //{
                //    action();
                //    _writeEvent.Reset();
                //}

                if (_readQueue.TryDequeue(out var func))
                {
                    var result = func();
                    _readResultQueue.Enqueue(result);
                    _readEvent.Set();
                }
            }
        }

        public List<string> Read(long bytesInMemory)
        {
            lock (_readLock)
            {
                _readEvent.Reset();
                _readQueue.Enqueue(() => _fileReader.ReadNextBath(bytesInMemory));
                _readEvent.Wait();
                while (true)
                {
                    if (_readResultQueue.TryDequeue(out var result))
                    {
                        return result;
                    }
                }
            }
        }

        public void Write(Action writeAction)
        {
            lock (_writeLock)
            {
                _writeTask = writeAction;
                _writeEvent.Reset();
                //_writeQueue.Enqueue(writeAction);
                _writeEvent.Wait();
            }
        }
    }
}
