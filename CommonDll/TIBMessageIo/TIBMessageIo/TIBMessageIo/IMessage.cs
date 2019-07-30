namespace TIBMessageIo
{
    public interface IMessage
    {
        //void InitialzeConfig();

        void Send(string sendMssage);
        string SendRequest(string sendMessage);
    }
}
