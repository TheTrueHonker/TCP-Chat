using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Net.Sockets;
using TCPChat.Common.Events;
using TCPChat.Common.Messages;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using TCP_Chat.Common.Network;

namespace TCPChat.Server
{
    class TCPServer
    {
        private readonly List<ClientInfo> clientInfos;
        private readonly TcpListener tcpListener;
        public EventHandler<NewMessageEventArgs> NewMessage;

        public TCPServer()
        {
            clientInfos = new List<ClientInfo>();
            tcpListener = new TcpListener(IPAddress.Any, Message.Port);
        }

        public void Start()
        {
            while (true)
            {
                tcpListener.Start();
                TcpClient client = tcpListener.AcceptTcpClient();
                var recieverThread = new Thread(() =>
                {
                    NetworkStream stream = client.GetStream();
                    while (true)
                    {
                        Message msg = NetworkManager.Deserialize(stream);
                        if (msg is StartupMessage startup)
                        {
                            Console.WriteLine("Client connected!");
                            OnNewMessage(new NewMessageEventArgs("CONNECTED", startup.Username),true);
                            string username = startup.Username;
                            clientInfos.Add(new ClientInfo(username, stream));
                            NetworkManager.Serialize(stream, new ConnectedMessage(), typeof(ConnectedMessage));
                        }
                        if (msg is MessageMessage message)
                        {
                            OnNewMessage(new NewMessageEventArgs(message.Message, message.Username),false);
                            foreach (ClientInfo info in clientInfos)
                            {
                                NetworkManager.Serialize(info.Stream, message, typeof(MessageMessage));
                            }
                        }
                    }
                });
                recieverThread.Start();
            }
        }

        protected virtual void OnNewMessage(NewMessageEventArgs e, bool systemMessage)
        {
            if (!systemMessage) e.Username = $"[{e.Username}]";
            NewMessage?.Invoke(this, e);
        }
    }
}
