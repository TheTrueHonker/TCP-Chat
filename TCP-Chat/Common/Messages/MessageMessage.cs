using System;

namespace TCPChat.Common.Messages
{
    [Serializable]
    public class MessageMessage : Message
    {
        public string Message;
        public string Username;

        public MessageMessage(string message, string username)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
