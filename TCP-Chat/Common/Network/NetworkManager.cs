using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TCPChat.Common.Messages;

namespace TCP_Chat.Common.Network
{
    static class NetworkManager
    {
        public static void Serialize(Stream stream, Message value, Type objType)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;

            StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII, 512, true);
            string json = JsonConvert.SerializeObject(value, objType, settings);
            Debug.WriteLine(json);
            streamWriter.WriteLine(json);
            streamWriter.Flush();
            streamWriter.Dispose();
        }

        public static Message Deserialize(Stream stream)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;

            StreamReader streamReader = new StreamReader(stream, Encoding.ASCII, true, 512, true);
            string json = streamReader.ReadLine();
            Debug.WriteLine(json);
            streamReader.Dispose();
            return JsonConvert.DeserializeObject<Message>(json, settings);
        }
    }
}
