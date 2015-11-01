using System;

namespace Homie.Client.ConnectionManagement
{
    public class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(string pStatusMessage)
        {
            StatusMessage = pStatusMessage;
        }

        public string StatusMessage { get; }
    }
}
