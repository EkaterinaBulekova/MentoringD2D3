using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    public class Continuation
    {
        public Task OnParentAnyWay(Task parent) => parent.ContinueWith((ant) =>
        {
            WriteTaskInTread(Thread.CurrentThread.ManagedThreadId, "regardless of the result \n" +
                              $"Parent succeed: {ant.IsCompletedSuccessfully}");

        }, CancellationToken.None, TaskContinuationOptions.AttachedToParent, TaskScheduler.Default);

        public Task OnParentNotSuccess(Task parent) => parent.ContinueWith((ant) =>
        {
            WriteTaskInTread(Thread.CurrentThread.ManagedThreadId, "finished without success \n" +
                              $"Parent succeed: {ant.IsCompletedSuccessfully}");

        }, CancellationToken.None, TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);

        public Task OnParentFault(Task parent) => parent.ContinueWith((ant) =>
        {
            WriteTaskInTread(Thread.CurrentThread.ManagedThreadId, "be finished with fail \n" +
                              $"Parent faulted: {ant.IsFaulted}");
            throw new ArgumentNullException("Some field in childOnParentFault task is null");

        }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        public Task OnParentCancel(Task parent) => parent.ContinueWith((ant) =>
        {
            WriteTaskInTread(Thread.CurrentThread.ManagedThreadId, "be cancelled \n" +
                              $"Parent canceled: {ant.IsCanceled}");
        }, TaskContinuationOptions.OnlyOnCanceled| TaskContinuationOptions.LongRunning);

        private void WriteTaskInTread(int threadId, string taskInfo)
        {
            Console.WriteLine($"Thread{threadId} - Start continuation task on parent task {taskInfo}");
        }    
    }
}
