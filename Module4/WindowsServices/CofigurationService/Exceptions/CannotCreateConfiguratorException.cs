using System;
using System.Runtime.Serialization;

namespace CofigurationService
{
    [Serializable]
    public class CannotCreateConfiguratorException : Exception
    {
        public CannotCreateConfiguratorException()
        {
        }

        public CannotCreateConfiguratorException(string message) : base(message)
        {
        }

        public CannotCreateConfiguratorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCreateConfiguratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}