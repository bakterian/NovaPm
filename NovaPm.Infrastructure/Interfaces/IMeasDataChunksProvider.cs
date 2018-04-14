using NovaPm.Infrastructure.Events;
using NovaPm.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace NovaPm.Infrastructure.Interfaces
{
    public interface IMeasDataChunksProvider
    {
        event MeasChunkReceivedEventHandler MeasReceivedEvent;

        IList<MeasChunk> GetMeasDataChunks(string measData, DateTime startTime);

        void StartReceivingMeasData(string serialPortId);

        void StopReceivingMeasData();
    }
}
