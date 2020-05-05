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

namespace TCPChat.Client
{
    class TCPClient
    {
        public string Username { get; }
        public string ServerIP { get; }
        //private readonly BinaryFormatter Formatter;
        private readonly JsonSerializer jsonSerializer;
        private readonly TcpClient Server;
        private readonly NetworkStream Stream;
        public event EventHandler<NewMessageEventArgs> NewMessage;

        public TCPClient(string username, string serverIP)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ServerIP = serverIP ?? throw new ArgumentNullException(nameof(serverIP));
            //Formatter = new BinaryFormatter();
            Server = new TcpClient(ServerIP, Message.Port);
            Stream = Server.GetStream();

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            jsonSerializer = JsonSerializer.Create(settings);
        }

        public void Start()
        {
            Serialize(new StartupMessage(Username));
            var recieverThread = new Thread(() =>
            {
                while (true)
                {
                    Message msg = Deserialize();
                    if(msg is ConnectedMessage)
                    {
                        Console.WriteLine("Connected!");
                    }
                    if(msg is MessageMessage message)
                    {
                        OnNewMessage(new NewMessageEventArgs(message.Message, message.Username));
                    }
                }
            });
            recieverThread.Start();
        }

        public void SendMessage(string message)
        {
            Serialize(new MessageMessage(message,Username));
            //Formatter.Serialize(Stream, new MessageMessage(message, Username));
        }

        public void Serialize(object value)
        {
            StreamWriter streamWriter = new StreamWriter(Stream);
            JsonWriter jsonWriter = new JsonTextWriter(streamWriter);
            jsonSerializer.Serialize(jsonWriter, value,typeof(Message));
        }

        public Message Deserialize()
        {
            StreamReader streamReader = new StreamReader(Stream);
            JsonReader jsonReader = new JsonTextReader(streamReader);
            return jsonSerializer.Deserialize<Message>(jsonReader);
        }

        protected virtual void OnNewMessage(NewMessageEventArgs e)
        {
            NewMessage?.Invoke(this, e);
        }
    }
}
