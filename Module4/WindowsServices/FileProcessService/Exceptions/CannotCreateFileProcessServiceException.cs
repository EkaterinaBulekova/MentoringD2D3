using System;
using System.Runtime.Serialization;

namespace FileProcessService
{
    [System.Serializable]
    public class CannotCreateFileProcessServiceException : Exception
    {
        public CannotCreateFileProcessServiceException()
        {
        }

        public CannotCreateFileProcessServiceException(string message) : base(message)
        {
        }

        public CannotCreateFileProcessServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCreateFileProcessServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}