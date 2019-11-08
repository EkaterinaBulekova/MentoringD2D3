using System;
using System.Runtime.Serialization;

namespace CofigurationService
{
    [Serializable]
    internal class ConfigurationNullExceplion : Exception
    {
        public ConfigurationNullExceplion()
        {
        }

        public ConfigurationNullExceplion(string message) : base(message)
        {
        }

        public ConfigurationNullExceplion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationNullExceplion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}