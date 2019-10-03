using System;
using System.Threading;
using TasksInterface;

namespace Task4
{
    public class RecursiveTreadsCreator : IStartable
    {
        private readonly int threadsCount;

        public RecursiveTreadsCreator(int threadsCount)
        {
            this.threadsCount = threadsCount;
        }

        public void StartNewThread(object count)
        {
            try
            {
                var countDown = (int)count;
                Console.WriteLine($"Current thread - {Thread.CurrentThread.ManagedThreadId} - counter => {countDown}");
                if (--countDown == 0) return;
                Console.WriteLine($"Counter decremented => {countDown}");
                var th = new Thread(StartNewThread);
                th.Start(countDown);
                th.Join();
            }
            catch (Exception ex)
            {
                throw new StartNewThreadException("Cannot start new thread-", ex);
            }
        }

        public void Start()
        {
            StartNewThread(threadsCount);
        }

    }
}

