using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using NovaPm.Infrastructure.Events;
using NovaPm.Infrastructure.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NovaPm.Control.Services
{
    public class MySerialReader : IMySerialReader, IDisposable
    {
        private SerialPort serialPort;
        private Queue<byte> recievedData = new Queue<byte>();
        private readonly IParameterStore _parameterStore;
        private BlockingCollection<string> _blockingCollection = new BlockingCollection<string>();
        private bool _listeningAtcive;

        public event DataReceivedEventHandler DataReceivedEvent;

        public MySerialReader(IParameterStore parameterStore)
        {
            _parameterStore = parameterStore;
        }

        public void ConfigureSerialPort(string port)
        {
            serialPort = new SerialPort(
                port,
                9600,
                (Parity)_parameterStore.SerialPortParity,
                _parameterStore.SerailPortDataCount,
                (StopBits)_parameterStore.SerailPortStopBits);
        }

        public void StartListening()
        {
            _listeningAtcive = true;
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.Open();
            if (!serialPort.IsOpen)
            {
                throw new AccessViolationException("Could not open COM port");
            }
            Task.Run(() => ProcessIncomingData());
        }

        public void StopListening()
        {
            _listeningAtcive = false;
            serialPort.DataReceived -= serialPort_DataReceived;
            serialPort.Close();
        }

        private void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[serialPort.BytesToRead];
            serialPort.Read(data, 0, data.Length);
            var quedStrings = data.Select(_ => string.Format("{0:X2}", _));
            var strChunk = string.Join("", quedStrings);

            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"StrChunk: {strChunk}, blockColCount: {_blockingCollection.Count}");
            #endif

            if (strChunk.Length >= _parameterStore.SerialDataThresholdValue)
            {
                _blockingCollection.Add(strChunk);
            }
        }

        private void ProcessIncomingData()
        {
            while(_listeningAtcive)
            {
                var strChunk = _blockingCollection.Take();
                DataReceivedEvent?.Invoke(this, new SerialDataEventArgs(strChunk));
            }
        }

        public void Dispose()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
        }
    }
}
