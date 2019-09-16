using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using LargeSort.FileSystem;
using Serilog;

namespace LargeSort.Sort.Logic.Merge
{
    internal class MultiWayFilesMerge
    {
        private readonly ILogger _logger;

        public MultiWayFilesMerge(ILogger logger)
        {
            _logger = logger;
        }

        public void Merge(string inputFolder, IWriter writer)
        {
            _logger.Information("Merge started");

            var files = Directory.GetFiles(inputFolder);
            _logger.Information($"Finds {files.Length} temp files");
            var watch = Stopwatch.StartNew();
            var filesList = new List<FileInMerge>(files.Length);
            foreach (var file in files)
            {
                var fileInMerge = new FileInMerge(file, _logger);
                if (fileInMerge.ReadNext())
                {
                    filesList.Add(fileInMerge);
                }
            }

            //while (filesList.Count != 0)
            //{
            //    FileInMerge min = filesList[0];
            //    for (int i = 1; i < filesList.Count; i++)
            //    {
            //        if (min > filesList[i])
            //        {
            //            min = filesList[i];
            //        }
            //    }

            //    writer.Append(Encoding.UTF8.GetBytes(min.CurrentValue));
            //    writer.Append(Encoding.UTF8.GetBytes(Environment.NewLine));
            //    if (!min.ReadNext())
            //    {
            //        filesList.Remove(min);
            //    }
            //}

            var sortedFiles = new SortedSet<FileInMerge>(filesList);

            var bufferedWriter = new BufferedWriter(256 * 1048576, writer);

            while (sortedFiles.Count != 0)
            {
                var min = sortedFiles.Min;
                sortedFiles.Remove(min);
                bufferedWriter.Write(Encoding.UTF8.GetBytes(min.CurrentValue));
                bufferedWriter.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
                if (min.ReadNext())
                {
                    sortedFiles.Add(min);
                }
            }

            bufferedWriter.Flush();
            writer.Flush();

            watch.Stop();
            _logger.Information($"Merge ended in {watch.Elapsed.ToString()}");
        }

        class BufferedWriter
        {
            private readonly IWriter _writer;
            private readonly MemoryStream _bufferStream;
            private readonly byte[] _buffer;
            private readonly object _lockObject = new object();

            public BufferedWriter(long bufferSize, IWriter writer)
            {
                _writer = writer;
                _buffer = new byte[bufferSize];
                _bufferStream = new MemoryStream(_buffer);
            }

            public void Write(byte[] toWrite)
            {
                lock (_lockObject)
                {
                    if (_bufferStream.Position + toWrite.Length >= _buffer.Length)
                    {
                        Flush();
                    }

                    _bufferStream.Write(toWrite.AsSpan());
                }
            }

            public void Flush()
            {
                lock (_lockObject)
                {
                    _writer.Append(_buffer.AsSpan(0, (int)_bufferStream.Position));
                    _bufferStream.Position = 0;
                }
            }
        }
    }
}
