using System;
using System.Threading.Tasks;
using TasksInterface;

namespace Task1
{
    /// <summary>
    /// Represents an asynchronous start iterators.
    /// </summary>
    public class Iterators : IStartable
    {
        private readonly int _iterationCount;
        private Task[] _tasks;

        /// <summary>
        /// Create new instance of Iterators class
        /// </summary>
        /// <param name="iterationCount"> Count of iteration that will do each iterator </param>
        public Iterators(int iterationCount)
        {
            _iterationCount = iterationCount;
        }

        /// <summary>
        /// Create and process tasks with iterator
        /// </summary>
        /// <param name="iteratorsCount"> Count of iterators that will start in process </param>
        public void Process(int iteratorsCount)
        {
            _tasks = new Task[iteratorsCount];

            for (int i = 0; i < iteratorsCount; i++)
            {
                _tasks[i] = Task.Factory.StartNew(StartIterator, i);
            }
            Task.WaitAll(_tasks);
        }

        private void StartIterator(object number)
        {
            for (int i = 0; i < _iterationCount; i++)
            {
                Console.WriteLine($"Task#{number}-iteration {i}");
            }
        }


        public void Start()
        {
            Process(100);
        }

    }
}