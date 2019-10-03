using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    public class Parent
    {
        private enum TaskResult
        {
            cancelation,
            exception,
            success
        }

        public CancellationTokenSource cancellationTokenSource = null;

        public Task WithCancelation()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            return Task.Factory.StartNew(() =>
            {
                WriteTaskAndTreadInfo(Thread.CurrentThread.ManagedThreadId, TaskResult.cancelation);
                Console.WriteLine("Do some work");
                token.WaitHandle.WaitOne();
                token.ThrowIfCancellationRequested();
                Console.WriteLine("Do more work");
            }, token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public Task WithException => Task.Factory.StartNew(() =>
        {
            WriteTaskAndTreadInfo(Thread.CurrentThread.ManagedThreadId, TaskResult.exception);
            throw new ArgumentNullException("Some field in parent task is null");
        }, CancellationToken.None,TaskCreationOptions.None, TaskScheduler.Default);

        public  Task WithSuccess => Task.Factory.StartNew(() =>
        {
            WriteTaskAndTreadInfo(Thread.CurrentThread.ManagedThreadId, TaskResult.success);
        }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

        private void WriteTaskAndTreadInfo(int threadId, TaskResult taskResult)
        {
            Console.WriteLine($"Thread{threadId} - Start parent task with {taskResult.ToString()}");
        }
    }
}
