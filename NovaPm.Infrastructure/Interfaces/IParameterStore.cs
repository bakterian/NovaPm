
namespace NovaPm.Infrastructure.Interfaces
{
    public interface IParameterStore
    {
        string MessageHeaderControlIdStr { get; set; }

        string MsgTail { get; set; }

        int ChunkSize {get; set;}

        int ChunkTimeInterval { get; set; }

        int SerailPortBaudRate { get; set; }

        int SerailPortDataCount { get; set; }

        int SerialPortParity { get; set; }

        int SerailPortStopBits { get; set; }

        int SerialDataThresholdValue { get; set; }

        string StartTimeStringNowNewLines { get; set; }

        int StartTimeInfoLength { get; set; }
    }
}