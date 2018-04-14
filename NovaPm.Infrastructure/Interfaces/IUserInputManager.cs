
namespace NovaPm.Infrastructure.Interfaces
{
    public interface IUserInputManager
    {
        bool LogFileWasProvided(string[] args);

        bool LogFileExists(string logFile);

        string GetLogFilePath(string logFile);
    }
}
