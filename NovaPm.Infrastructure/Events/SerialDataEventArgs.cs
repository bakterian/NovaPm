using System;
using System.Collections.Generic;

namespace NovaPm.Infrastructure.Events
{
    public class SerialDataEventArgs : EventArgs
    {
        public string ReceivedStrChunk { get; set; }

        public SerialDataEventArgs(string receivedStrChunk)
        {
            ReceivedStrChunk = receivedStrChunk;
        }
    }
}
