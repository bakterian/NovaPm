using System;

namespace NovaPm.Infrastructure.Interfaces
{
    public interface IStreamHelper
    {
        int GetMsgHeaderStartIndex(string capturedData);

        string RemoveUnwantedChars(string capturedData);

        bool ContainsStartTime(string capturedData);

        DateTime GetStartTime(string capturedData);
    }
}
