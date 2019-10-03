using System;
using System.Collections.Generic;
using System.Text;

namespace Task4
{
    [Serializable]
    internal class StartNewThreadException : InvalidOperationException
    {
        public StartNewThreadException()
        {
        }

        public StartNewThreadException(string message) : base(message)
        {
        }

        public StartNewThreadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
