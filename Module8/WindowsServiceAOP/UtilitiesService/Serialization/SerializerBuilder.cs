using System;

namespace UtilitiesService.Serialization
{
    public static class SerializerBuilder
    {
        public static ISerializer CreateSerializer(SerializeType type)
        {
            switch (type)
            {
                    case SerializeType.Xml:
                        return new XmlSerializer();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
