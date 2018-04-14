using System;
using NovaPm.Infrastructure.Interfaces;

namespace NovaPm.Control.Services
{
    public class ParameterStore : IParameterStore
    {
        public string MessageHeaderControlIdStr { get; set; } = "AAC0";

        public string MsgTail { get; set; } = "AB";

        public int ChunkSize { get; set; } = 20;

        public int ChunkTimeInterval { get; set; } = 1;

        public int SerailPortBaudRate { get; set; } = 9600;

        public int SerailPortDataCount { get; set; } = 8;

        public int SerialPortParity { get; set; } = 0; //None

        public int SerailPortStopBits { get; set; } = 1; //One

        public int SerialDataThresholdValue { get; set; } = 20;

        public string StartTimeStringNowNewLines { get; set; } = "Terminal log fileDate: ";

        public int StartTimeInfoLength { get; set; } = 44;
    }
}
