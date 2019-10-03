using System;
using System.Threading;
using TasksInterface;

namespace Task5
{
    public class RecursiveTreadPoolCreator : IStartable
    {
        private readonly int _threadsCount;
        private Semaphore _semaphore;


        public RecursiveTreadPoolCreator(int threadsCount)
        {
            _threadsCount = threadsCount;
            _semaphore = new Semaphore(1, threadsCount);
        }

        public void StartNewThread(object count)
        {
            try
            {
                _semaphore.WaitOne();
                var countDown = (int)count;
                Console.WriteLine($"Current thread - {Thread.CurrentThread.ManagedThreadId} - counter => {countDown}");
                if (--countDown == 0) return;
                Console.WriteLine($"Counter decremented => {countDown}");
                ThreadPool.QueueUserWorkItem(StartNewThread, countDown);
            }
            catch (Exception ex)
            {
                throw new StartNewThreadPoolItemException("Cannot create new thread", ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Start()
        {
            _semaphore.WaitOne();
            ThreadPool.QueueUserWorkItem(StartNewThread, _threadsCount);
            _semaphore.Release();
            Thread.Sleep(1000);
        }
    }
}
