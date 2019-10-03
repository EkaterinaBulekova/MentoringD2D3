using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TasksInterface;

namespace Task7
{
    public class ParentContinuation : IStartable
    {
        private Parent parent;
        private Continuation continuation;
 
        public ParentContinuation()
        {
            parent = new Parent();
            continuation = new Continuation();
        }

        public void StartAllContinuations(Task antecedent)
        {
            Task childOnParentAnyWay = continuation.OnParentAnyWay(antecedent);

            Task childOnParentNotSuccess = continuation.OnParentNotSuccess(antecedent);

            Task childOnParentFault = continuation.OnParentFault(antecedent);

            Task childOnParentCancel = continuation.OnParentCancel(antecedent);

            //Thread.Sleep(1000);
            parent.cancellationTokenSource?.Cancel();

            try
            { 
                antecedent.Wait();
            }
            catch (AggregateException ae)
            {
                Trace.WriteLine(" Enter Exception Handler: ");
                foreach (Exception e in ae.InnerExceptions)
                    Trace.WriteLine(e.Message);
                Trace.WriteLine(" Exit Exception Handler: ");
            }
            finally
            {
                Console.WriteLine();
            }
        }

        public void Start()
        {
            StartAllContinuations(parent.WithSuccess);
            StartAllContinuations(parent.WithException);
            StartAllContinuations(parent.WithCancelation());
        }
    }
}
