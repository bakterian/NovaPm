using NovaPm.Infrastructure.Models;

namespace NovaPm.Infrastructure.Events
{
    public class MeasChunkEventArgs
    {
        public MeasChunk Chunk {get;set;}

        public MeasChunkEventArgs(MeasChunk chunk)
        {
            Chunk = chunk;
        }
    }
}
