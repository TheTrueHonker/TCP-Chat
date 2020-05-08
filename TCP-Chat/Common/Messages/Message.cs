using Newtonsoft.Json;
using System;

namespace TCPChat.Common.Messages
{
    [Serializable]
    public abstract class Message
    {
        public static int Port = 25542;
    }
}
