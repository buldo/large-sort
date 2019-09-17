using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
//using System.Threading.Channels;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic
{
    internal class BatchFileReader : IDisposable
    {
        private readonly ManualResetEventSlim _readEvent;
        private readonly StreamReader _reader;

        //private readonly Channel<string> _queue = Channel.CreateUnbounded<string>(new UnboundedChannelOptions(){SingleReader = true, SingleWriter = true, AllowSynchronousContinuations = true});
        private int _prevSize = 0;

        private bool _ended = false;

        private readonly object _readLock = new object();

        public BatchFileReader(string fileName, ManualResetEventSlim readEvent)
        {
            _readEvent = readEvent;
            _reader = new StreamReader(fileName, Encoding.UTF8, false, 4906);
        }

        public List<CompositeString> ReadNextBath(long bytesInMemory)
        {
            //var ret = new List<CompositeString>(_prevSize == 0 ? 10000 : _prevSize);
            var ret = new List<CompositeString>(10000);

            string line;
            long reads = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                reads++;
                ret.Add(new CompositeString(line));
                    if (reads >= 10000)
                    {
                        break;
                    }
            }

            //_prevSize = ret.Count;


            //long reads = 0;
            //while (!_ended)
            //{
            //    if (_queue.Reader.TryRead(out var line))
            //    {
            //        reads += line.Length * 4 + 18 + 4 + 8;
            //        ret.Add(new CompositeString(line));
            //        if (reads > bytesInMemory)
            //        {
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        PopulateNextGb();
            //    }
            //}

            //_prevSize = ret.Count;

            return ret;
        }

        //private void PopulateNextGb()
        //{
        //    lock (_readLock)
        //    {
        //        long bytesInMemory = (long) 1024 * 1048576;
        //        string line;
        //        long reads = 0;
        //        while ((line = _reader.ReadLine()) != null)
        //        {
        //            //_readEvent.Wait();
        //            reads += line.Length * 4 + 18;
        //            while (!_queue.Writer.TryWrite(line))
        //            {
        //            }

        //            if (reads > bytesInMemory)
        //            {
        //                break;
        //            }
        //        }

        //        if (line == null)
        //        {
        //            _ended = true;
        //        }
        //    }
        //}

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
