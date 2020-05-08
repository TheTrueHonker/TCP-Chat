using System;
using TCPChat.Server;
using TCPChat.Client;
using System.Diagnostics;
using TCPChat.Common.Events;

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
            server.NewMessage = DisplayMessage;
            server.Start();
        }

        static void StartClient()
        {
            Console.WriteLine("Choose a username:");
            Console.Write(">");
            string username = Console.ReadLine();
            Console.WriteLine("\nServer IP:");
            Console.Write(">");
            string serverIP = Console.ReadLine();
            TCPClient client = new TCPClient(username, serverIP);
            client.NewMessage += DisplayMessage;
            client.Start();
            while (true)
            {
                client.SendMessage(Console.ReadLine());
            }
        }

        static void DisplayMessage(object senser, NewMessageEventArgs e)
        {
            var backupColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{e.Username}] {e.Message}");
            Console.ForegroundColor = backupColor;
        }
    }
}
