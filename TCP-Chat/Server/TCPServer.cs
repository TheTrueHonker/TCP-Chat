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

namespace TCPChat.Server
{
    class TCPServer
    {
        private readonly List<ClientInfo> clientInfos;
        //private readonly BinaryFormatter formatter;
        private readonly TcpListener tcpListener;
        private readonly JsonSerializer jsonSerializer;
        public EventHandler<NewMessageEventArgs> NewMessage;

        public TCPServer()
        {
            clientInfos = new List<ClientInfo>();
            //formatter = new BinaryFormatter();
            tcpListener = new TcpListener(IPAddress.Any, Message.Port);
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            jsonSerializer = JsonSerializer.Create(settings);
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
                        if(stream.DataAvailable)
                        {
                            Debug.WriteLine(stream.ReadByte() + "\nGetting Something");
                            Debug.WriteLine(stream.Length);
                        }
                        Message msg = Deserialize(stream);
                        if (msg is StartupMessage startup)
                        {
                            OnNewMessage(new NewMessageEventArgs("CONNECTED", startup.Username),true);
                            string username = startup.Username;
                            clientInfos.Add(new ClientInfo(username, stream));
                            Serialize(stream, new ConnectedMessage());
                        }
                        if (msg is MessageMessage message)
                        {
                            OnNewMessage(new NewMessageEventArgs(message.Message, message.Username),false);
                            foreach (ClientInfo info in clientInfos)
                            {
                                Serialize(info.Stream, message);
                            }
                        }
                    }
                });
                recieverThread.Start();
            }
        }

        public void Serialize(Stream stream, object value)
        {
            StreamWriter streamWriter = new StreamWriter(stream);
            JsonWriter jsonWriter = new JsonTextWriter(streamWriter);
            jsonSerializer.Serialize(jsonWriter, value,typeof(Message));
        }

        public Message Deserialize(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            JsonReader jsonReader = new JsonTextReader(streamReader);
            Message message = jsonSerializer.Deserialize<Message>(jsonReader);
            Console.WriteLine("Blocker");
            return message;
        }

        protected virtual void OnNewMessage(NewMessageEventArgs e, bool systemMessage)
        {
            if (!systemMessage) e.Username = $"[{e.Username}]";
            NewMessage?.Invoke(this, e);
        }
    }
}
