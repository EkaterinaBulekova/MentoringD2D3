using System;
using System.Runtime.Serialization;

namespace FileProcessService
{
    [System.Serializable]
    public class CannotStartFileProcessQueueException : Exception
    {
        public CannotStartFileProcessQueueException()
        {
        }

        public CannotStartFileProcessQueueException(string message) : base(message)
        {
        }

        public CannotStartFileProcessQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotStartFileProcessQueueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}