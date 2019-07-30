namespace EQPIO.RabbitMQInterface.Parser
{
	public interface IMessageParser
	{
		byte[] ObjectToByteArray(object obj);

		object ByteArrayToObject(byte[] arr);
	}
}
