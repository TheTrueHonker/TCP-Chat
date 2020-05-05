using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace TCPChat.Server
{
    [Serializable]
    class ClientInfo
    {
        public string Username;
        public NetworkStream Stream;

        public ClientInfo(string username, NetworkStream stream)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
    }
}
