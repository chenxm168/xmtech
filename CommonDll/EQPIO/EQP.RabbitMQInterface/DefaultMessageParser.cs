
namespace EQPIO.RabbitMQInterface.Parser.Impl
{
    using EQPIO.RabbitMQInterface.Parser;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    internal class DefaultMessageParser : IMessageParser
    {
        public object ByteArrayToObject(byte[] byteArray)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return null;
        }

        public byte[] ObjectToByteArray(object obj)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return null;
        }
    }
}
