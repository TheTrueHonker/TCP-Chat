using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChat.Common.Events
{
    class NewMessageEventArgs : EventArgs
    {
        public string Message;
        public string Username;

        public NewMessageEventArgs(string message, string username)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
