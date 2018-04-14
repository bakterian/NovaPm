using NovaPm.Infrastructure.Models;
using System;

namespace NovaPm.Infrastructure.Interfaces
{
    public interface IMeasChunkCreator
    {
        MeasChunk CreateMeasChunk(string chunkData, DateTime chunkCreationTime);

        bool IsValidStrChunk(string measStrChunk);
    }
}

