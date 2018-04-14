using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaPm.Infrastructure.Extensions
{
    public static class ExtensionMethods
    {
        public static string RemoveSpaces(this String str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static string RemoveNewLines(this String str)
        {
            var processedStr = str.Replace("\r", string.Empty);
            processedStr = processedStr.Replace("\n", string.Empty);
            return processedStr;
        }

        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
