using System;
using System.Runtime.Serialization;

namespace FileProcessService
{
    [System.Serializable]
    internal class CannotCreateFileProcessQueueException : Exception
    {
        public CannotCreateFileProcessQueueException()
        {
        }

        public CannotCreateFileProcessQueueException(string message) : base(message)
        {
        }

        public CannotCreateFileProcessQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCreateFileProcessQueueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}