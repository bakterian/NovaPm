using NovaPm.Infrastructure.Extensions;
using NovaPm.Infrastructure.Interfaces;
using System;
using System.Diagnostics;
using System.Globalization;

namespace NovaPm.Control.Services
{
    public class StreamHelper : IStreamHelper
    {
        private readonly IParameterStore _parameterStore;

        public StreamHelper(IParameterStore parameterStore)
        {
            _parameterStore = parameterStore;
        }

        public int GetMsgHeaderStartIndex(string capturedData)
        {
            return capturedData.ToUpper().IndexOf(_parameterStore.MessageHeaderControlIdStr.ToUpper());
        }

        public string RemoveUnwantedChars(string capturedData)
        {
            return capturedData
                    .RemoveSpaces()
                    .RemoveNewLines();
        }

        public bool ContainsStartTime(string capturedData)
        {
            var foundStartTime = false;

            var capturedDataRefactored = capturedData
                            .RemoveNewLines()
                            .ToUpper();

            var dateStartIndex = GetTimeStartIndex(capturedDataRefactored);

            if ((dateStartIndex != -1) && GetDateTime(capturedDataRefactored, dateStartIndex).HasValue)
            {
                foundStartTime = true;
            }

            return foundStartTime;
        }

        public DateTime GetStartTime(string capturedData)
        {
            var capturedDataRefactored = capturedData
                            .RemoveNewLines()
                            .ToUpper();

            var dateStartIndex = GetTimeStartIndex(capturedDataRefactored);

            return GetDateTime(capturedDataRefactored, dateStartIndex).Value;
        }

        private DateTime? GetDateTime(string capturedData, int startIndex)
        {
            DateTime? dateTime = null;

            var dateTimeStr = capturedData.Substring(startIndex, GetDateTimeStrLength());

            if (DateTime.TryParseExact(dateTimeStr, "yyyy-MM-dd - HH:mm:ss",CultureInfo.InvariantCulture,DateTimeStyles.AssumeLocal, out var resultingDateTime))
            {
                dateTime = resultingDateTime;
            }

            return dateTime;
        }

        private int GetTimeStartIndex(string capturedData)
        {
            var index = capturedData.IndexOf(_parameterStore.StartTimeStringNowNewLines.ToUpper());

            if (index != -1) index += _parameterStore.StartTimeStringNowNewLines.Length;

            return index;
        }

        private int GetDateTimeStrLength()
        {
            return (_parameterStore.StartTimeInfoLength - _parameterStore.StartTimeStringNowNewLines.Length);
        }
    }
}