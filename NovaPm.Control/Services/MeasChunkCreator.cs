using NovaPm.Infrastructure.Interfaces;
using NovaPm.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NovaPm.Control.Services
{
    public class MeasChunkCreator : IMeasChunkCreator
    {
        private readonly IParameterStore _parameterStore;

        public MeasChunkCreator(IParameterStore parameterStore)
        {
            _parameterStore = parameterStore;
        }
        public MeasChunk CreateMeasChunk(string chunkData, DateTime chunkCreationTime)
        {
            var dataCollection = GetDataCollecion(chunkData);

            var measChunk = new MeasChunk();
            measChunk.TimeDate = chunkCreationTime;
            measChunk.Pm2_5Amount = Convert.ToSingle((dataCollection[3] << 8) | dataCollection[2]) / 10.0f;
            measChunk.Pm10Amount = Convert.ToSingle((dataCollection[5] << 8) | dataCollection[4]) / 10.0f;
            measChunk.ID = ((dataCollection[7] << 8) | dataCollection[6]);
            measChunk.Checksum = dataCollection[8];

            return measChunk;
        }

        public bool IsValidStrChunk(string measStrChunk)
        {
            var isValidStrChunk = false;

            if (measStrChunk.Length.Equals(_parameterStore.ChunkSize) &&
                IsValidMsgHeader(measStrChunk.Substring(0, 4)) &&
                IsValidMsgTail(measStrChunk.Substring(18, 2)) &&
                IsValidHexData(measStrChunk) &&
                HasValidChecksum(measStrChunk))
            {
                isValidStrChunk = true;
            }

            return isValidStrChunk;
        }

        private bool IsValidMsgHeader(string headerStr)
        {
            return headerStr.Equals(_parameterStore.MessageHeaderControlIdStr);
        }

        private bool IsValidMsgTail(string tailStr)
        {
            return tailStr.Equals(_parameterStore.MsgTail);
        }

        private bool IsValidHexData(string strChunk)
        {
            var isValidHexData = true;
            var culture = CultureInfo.CurrentCulture;
            for (int i = 0; i < 20; i=i+2)
            {
               var hexStr = strChunk.Substring(i, 2);

                if (!int.TryParse(hexStr, NumberStyles.HexNumber, culture.NumberFormat, out var result))
                {
                    isValidHexData = false;
                    break;
                }
            }
            return isValidHexData;
        }

        private bool HasValidChecksum(string strChunk)
        {
            var dataCollection = GetDataCollecion(strChunk);

            return (dataCollection.Skip(2).Take(6).Sum() & 0xFF).Equals(dataCollection[8]);
        }

        private IList<int> GetDataCollecion(string strChunk)
        {
            var culture = CultureInfo.CurrentCulture;
            var dataCollection = new List<int>();
            for (int i = 0; i < 20; i = i + 2)
            {
                var hexStr = strChunk.Substring(i, 2);

                if (int.TryParse(hexStr, NumberStyles.HexNumber, culture.NumberFormat, out var result))
                {
                    dataCollection.Add(result);
                }
            }

            return dataCollection;
        }
    }
}
