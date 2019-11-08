using System;
using System.Runtime.Serialization;

namespace FileProcessService
{
    [System.Serializable]
    internal class CannotEnqueueFileProcessQueueException : Exception
    {
        public CannotEnqueueFileProcessQueueException()
        {
        }

        public CannotEnqueueFileProcessQueueException(string message) : base(message)
        {
        }

        public CannotEnqueueFileProcessQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotEnqueueFileProcessQueueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}