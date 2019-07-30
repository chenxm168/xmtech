
namespace EQPIO.RabbitMQInterface.Parser.Impl
{
    using HF.BC.JSON;
    using EQPIO.RabbitMQInterface.Parser;
    using System;
    using System.Text;

    public class JsonMessageParser<T> : IMessageParser
    {
        private JSONConverter<T> _jsonParser;

        public JsonMessageParser()
        {
            this._jsonParser = new JSONConverter<T>();
        }

        public object ByteArrayToObject(byte[] arr)
        {
            return this._jsonParser.StringToObject(Encoding.UTF8.GetString(arr));
        }

        public byte[] ObjectToByteArray(object obj)
        {
            return Encoding.UTF8.GetBytes(this._jsonParser.ObjectToString((T)obj));
        }
    }
}
