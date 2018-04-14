using NovaPm.Infrastructure.Interfaces;
using System.Collections.Generic;
using NovaPm.Infrastructure.Models;
using NovaPm.Infrastructure.Events;
using System.Text;
using NovaPm.Infrastructure.Extensions;
using System;

namespace NovaPm.Control.Services
{
    public class MeasDataChunksProvider : IMeasDataChunksProvider
    {
        private readonly IMeasChunkCreator _measChunkCreator;
        private readonly IParameterStore _parameterStore;
        private readonly IMySerialReader _mySerialReader;

        public event MeasChunkReceivedEventHandler MeasReceivedEvent;

        public MeasDataChunksProvider
            (
            IMeasChunkCreator measChunkCreator, 
            IParameterStore parameterStore,
            IMySerialReader mySerialReader
            )
        {
            _measChunkCreator = measChunkCreator;
            _parameterStore = parameterStore;
            _mySerialReader = mySerialReader;
        }

        public IList<MeasChunk> GetMeasDataChunks(string measData, DateTime startTime)
        {
            var chunks = new List<MeasChunk>();
            var currIndex = 0;
            var chunkId = 0U;
            while( (currIndex + _parameterStore.ChunkSize) < measData.Length)
            {
                var chunkStrData = measData.Substring(currIndex, _parameterStore.ChunkSize);

                if(_measChunkCreator.IsValidStrChunk(chunkStrData))
                {
                    var dateTime = startTime.AddSeconds(chunkId * _parameterStore.ChunkTimeInterval);
                    var measChunk = _measChunkCreator.CreateMeasChunk(chunkStrData, dateTime);
                    chunks.Add(measChunk);
                }

                currIndex += _parameterStore.ChunkSize;
                chunkId++;
            }

            return chunks;
        }

        public void StartReceivingMeasData(string serialPortId)
        {
            _mySerialReader.ConfigureSerialPort(serialPortId);

            SubsribeForDataReceive();

            _mySerialReader.StartListening();
        }

        public void StopReceivingMeasData()
        {
            UnsubsribeForDataReceive();

            _mySerialReader.StopListening();
        }

        private void OnSerialDataIncoming(object sender, SerialDataEventArgs e)
        {
            var receivedString = e.ReceivedStrChunk;

            if (receivedString.Length == _parameterStore.ChunkSize)
            {             
                if (_measChunkCreator.IsValidStrChunk(receivedString))
                {
                    var measChunk = _measChunkCreator.CreateMeasChunk(receivedString, DateTime.Now);
                    MeasReceivedEvent?.Invoke(this, new MeasChunkEventArgs(measChunk));
                }                                  
            }
        }

        private void SubsribeForDataReceive()
        {
            _mySerialReader.DataReceivedEvent -= OnSerialDataIncoming;
            _mySerialReader.DataReceivedEvent += OnSerialDataIncoming;
        }

        private void UnsubsribeForDataReceive()
        {
            _mySerialReader.DataReceivedEvent += OnSerialDataIncoming;
        }
    }
}
