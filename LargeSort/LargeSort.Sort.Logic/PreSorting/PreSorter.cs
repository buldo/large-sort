using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic.PreSorting
{
    internal class PreSorter
    {
        private readonly string _inputFile;
        private readonly List<Task> _sortTasks = new List<Task>();
        private readonly List<Task> _writeTasks = new List<Task>();
        private readonly ConcurrentBag<SortingTask> _tasksForReuse = new ConcurrentBag<SortingTask>();
        private object _ioLock = new object();

        public PreSorter(string inputFile)
        {
            _inputFile = inputFile;
        }

        public void PreSort(string tempFolder, int count)
        {
            using (var reader =
                new StreamReader(new FileStream(_inputFile, FileMode.Open, FileAccess.Read, FileShare.None)))
            {
                var semaphore = new SemaphoreSlim(count, count);
                while (true)
                {
                    var task = GetTask(reader);

                    lock (_ioLock)
                    if (!task.Read())
                    {
                        break;
                    }

                    semaphore.Wait();

                    var sortTask = task.Sort();
                    sortTask.ContinueWith(task1 =>
                    {
                        var t = Task.Run(() =>
                        {
                            lock (_ioLock)
                                task1.Result.Write(Path.Combine(tempFolder, Path.GetRandomFileName()), semaphore);
                            _tasksForReuse.Add(task1.Result);
                        });
                        _writeTasks.Add(t);
                    });
                    _sortTasks.Add(sortTask);
                }

                Task.WaitAll(_sortTasks.ToArray());
                Task.WaitAll(_writeTasks.ToArray());
                _sortTasks.Clear();
                _writeTasks.Clear();
            }
        }


        private SortingTask GetTask(StreamReader reader)
        {
            if (_tasksForReuse.TryTake(out var task))
            {
                return task;
            }

            return new SortingTask(reader);
        }
    }
}
