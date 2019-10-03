using System;

namespace Task5
{
    [Serializable]
    internal class StartNewThreadPoolItemException : InvalidOperationException
    {
        public StartNewThreadPoolItemException()
        {
        }

        public StartNewThreadPoolItemException(string message) : base(message)
        {
        }

        public StartNewThreadPoolItemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
