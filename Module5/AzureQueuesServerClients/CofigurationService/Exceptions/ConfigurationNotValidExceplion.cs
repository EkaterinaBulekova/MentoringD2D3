using System;
using System.Runtime.Serialization;

namespace CofigurationService
{
    [Serializable]
    internal class ConfigurationNotValidExceplion : Exception
    {
        public ConfigurationNotValidExceplion()
        {
        }

        public ConfigurationNotValidExceplion(string message) : base(message)
        {
        }

        public ConfigurationNotValidExceplion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationNotValidExceplion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}