using System;

namespace TCPChat.Common.Messages
{
    [Serializable]
    public class StartupMessage : Message
    {
        public string Username { get; set; }

        public StartupMessage(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
