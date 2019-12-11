using System;

namespace LoggingService.Serialization
{
    public static class SerializatorBuilder
    {
        public static ISerializer CreateSerializer(SerializatorType type)
        {
            switch (type)
            {
                    case SerializatorType.Xml:
                        return new XmlSerializator();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
