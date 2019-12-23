namespace Somfic.Logging_old
{
    public class TcpMessage
    {
        public TcpMessage(MessageType messageType, string content)
        {
            MessageType = messageType;
            Content = content;
        }


        public MessageType MessageType { get; private set; }
        public string Content { get; private set; }
    }
}