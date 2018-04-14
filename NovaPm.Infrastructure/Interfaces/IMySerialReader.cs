using NovaPm.Infrastructure.Events;
using System;

namespace NovaPm.Infrastructure.Interfaces
{
    public interface IMySerialReader
    {
        event DataReceivedEventHandler DataReceivedEvent;

        void ConfigureSerialPort(string port);

        void StartListening();

        void StopListening();
    }
}
