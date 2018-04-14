using System;
using NovaPm.Infrastructure.Interfaces;
using System.IO;

namespace NovaPm.Control.Services
{
    public class UserInputManager : IUserInputManager
    {
        public bool LogFileWasProvided(string[] args)
        {
            return (args.Length == 1 && !string.IsNullOrEmpty(args[0]));
        }

        public bool LogFileExists(string logFile)
        {
            return (File.Exists(logFile) || File.Exists($"{Directory.GetCurrentDirectory()}\\{logFile}"));
        }

        public string GetLogFilePath(string logFile)
        {
            return (File.Exists(logFile) ? logFile : $"{Directory.GetCurrentDirectory()}\\{logFile}");
        }
    }
}
