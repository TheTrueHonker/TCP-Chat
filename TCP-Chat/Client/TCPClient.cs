using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Net.Sockets;
using TCPChat.Common.Events;
using TCPChat.Common.Messages;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using TCP_Chat.Common.Network;

namespace TCPChat.Client
{
    class TCPClient
    {
        public string Username { get; }
        public string ServerIP { get; }
        private readonly TcpClient Server;
        private readonly NetworkStream Stream;
        public event EventHandler<NewMessageEventArgs> NewMessage;

        public TCPClient(string username, string serverIP)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ServerIP = serverIP ?? throw new ArgumentNullException(nameof(serverIP));
            Server = new TcpClient(ServerIP, Message.Port);
            Stream = Server.GetStream();
        }

        public void Start()
        {
            NetworkManager.Serialize(Stream, new StartupMessage(Username),typeof(StartupMessage));
            var recieverThread = new Thread(() =>
            {
                while (true)
                {
                    Message msg = NetworkManager.Deserialize(Stream);
                    if (msg is ConnectedMessage)
                    {
                        Console.WriteLine("Connected!");
                    }
                    if (msg is MessageMessage message)
                    {
                        OnNewMessage(new NewMessageEventArgs(message.Message, message.Username));
                    }
                }
            });
            recieverThread.Start();
        }

        public void SendMessage(string message)
        {
            NetworkManager.Serialize(Stream, new MessageMessage(message,Username),typeof(MessageMessage));
        }

        protected virtual void OnNewMessage(NewMessageEventArgs e)
        {
            NewMessage?.Invoke(this, e);
        }
    }
}
