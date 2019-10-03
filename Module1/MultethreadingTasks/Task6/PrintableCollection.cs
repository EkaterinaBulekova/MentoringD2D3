using System;
using System.Linq;
using System.Threading;

namespace Task6
{
    /// <summary>
    /// Represents collection that will print all items durin initialization 
    /// </summary>
    public class PrintableCollection : IDisposable
    {
        static EventWaitHandle _workerReady = new AutoResetEvent(false);
        static EventWaitHandle _itemAdded = new AutoResetEvent(false);

        private Thread _worker;
        private ThreadSafeListWithLock<string> _collection = new ThreadSafeListWithLock<string>();

        /// <summary>
        /// Create new instance of RecursiveTreadPoolCreator class
        /// </summary>
        public PrintableCollection()
        {
            _worker = new Thread(PrintToConsole);
            _worker.Start();
        }

        public void AddItem(string item)
        {
            _workerReady.WaitOne();
            _collection.Add(item);
            _itemAdded.Set();
        }

        public void Dispose()
        {
            AddItem(null);     
            _worker.Join();
            _itemAdded.Close();
            _workerReady.Close();
        }

        private void PrintToConsole()
        {
            while (true)
            {
                _workerReady.Set();
                _itemAdded.WaitOne();
                if (_collection.Count > 0)
                {
                    var item = _collection.Last();
                    if (item == null) return;
                    Console.WriteLine("Added: " + _collection.Last());
                }
            }
        }
    }
}
