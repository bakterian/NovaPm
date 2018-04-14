
using CommandLine;
using System;
using System.Collections.Generic;

namespace NovaPm.Infrastructure.Interfaces
{
    public interface ICommandLineParser : IDisposable
    {
        Parser CmdParser { get; }

        ParserSettings Settings { get; }

        ParserResult<T> ParseArguments<T>(IEnumerable<string> args);

        ParserResult<T> ParseArguments<T>(Func<T> factory, IEnumerable<string> args) where T : new();

        ParserResult<object> ParseArguments(IEnumerable<string> args, params Type[] types);
    }

}
