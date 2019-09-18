using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LargeSort.Sort.Logic
{
    public class Sorter
    {
        private readonly string _inputFile;
        private readonly List<Task> _sortTasks = new List<Task>();
        private readonly List<Task> _writeTasks = new List<Task>();
        private readonly ConcurrentBag<SortingTask> _tasksForReuse = new ConcurrentBag<SortingTask>();

        public Sorter(string inputFile)
        {
            _inputFile = inputFile;
        }

        public void Sort(string outFile, int count)
        {
            var tempFolder = Path.Combine(Path.GetDirectoryName(outFile), Path.GetRandomFileName());
            Directory.CreateDirectory(tempFolder);
            using (var reader =
                new StreamReader(new FileStream(_inputFile, FileMode.Open, FileAccess.Read, FileShare.None)))
            {
                var semaphore = new SemaphoreSlim(count, count);
                while (true)
                {
                    var task = GetTask(reader);

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
                            task1.Result.Write(Path.Combine(tempFolder, Path.GetRandomFileName()), semaphore);
                            _tasksForReuse.Add(task1.Result);
                        });
                        _writeTasks.Add(t);
                    });
                    _sortTasks.Add(sortTask);
                }

                Task.WaitAll(_sortTasks.ToArray());
                Task.WaitAll(_writeTasks.ToArray());
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