using System;
using TCPChat.Server;
using TCPChat.Client;

namespace TCPChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please choose:\n" +
                "[1] Start Server\n" +
                "[2] Start Client");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    StartServer();
                    break;
                case "2":
                    Console.Clear();
                    StartClient();
                    break;
            }
        }

        static void StartServer()
        {
            TCPServer server = new TCPServer();
            server.Start();
        }

        static void StartClient()
        {
            Console.WriteLine("Choose a username:");
            string username = Console.ReadLine();
            Console.WriteLine("Server IP:");
            string serverIP = Console.ReadLine();
            TCPClient client = new TCPClient(username, serverIP);
            client.Start();
        }
    }
}
