using System;
using System.Runtime.Serialization;

namespace FileQueueService
{
    [Serializable]
    internal class SendFileToMessageException : Exception
    {
        public SendFileToMessageException()
        {
        }

        public SendFileToMessageException(string message) : base(message)
        {
        }

        public SendFileToMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SendFileToMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}