using System;
using NovaPm.Infrastructure.Interfaces;
using System.IO;
using System.Collections.Generic;
using NovaPm.Infrastructure.Models;
using CommandLine;
using NovaPm.Infrastructure.Events;
using CommandLine.Text;

namespace NovaPm.Control.Services
{
    public class ConsoleRunner : IConsoleRunner
    {
        private readonly IStreamHelper _streamHelper;
        private readonly IMeasDataChunksProvider _measDataChunksProvider;
        private readonly IUserInputManager _userInputManager;
        private readonly ICommandLineParser _cmdParser;

        private Options CmdOptions { get; set; }

        public ConsoleRunner(
            IStreamHelper streamHelper,
            IMeasDataChunksProvider measDataChunksProvider,
            IUserInputManager userInputManager,
            ICommandLineParser cmdParser)
        {
            _streamHelper = streamHelper;
            _measDataChunksProvider = measDataChunksProvider;
            _userInputManager = userInputManager;
            _cmdParser = cmdParser;
        }

        public void ProcessUserInput(string[] inputArgs)
        {
            var parserResults =  _cmdParser.CmdParser.ParseArguments<Options>(inputArgs)
               .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
               .WithNotParsed((errs) => HandleParseError(errs));

            if (!string.IsNullOrEmpty(CmdOptions?.LogFileToBeParsed))
            {
                Console.WriteLine($"LogFile to parse: {CmdOptions.LogFileToBeParsed}");
                if (_userInputManager.LogFileExists(CmdOptions.LogFileToBeParsed))
                {
                    var filePath = _userInputManager.GetLogFilePath(CmdOptions.LogFileToBeParsed);
                    using (var streamReader = new StreamReader(File.OpenRead(filePath)))
                    {
                        var bigString = streamReader.ReadToEnd();
                        var cleandedString = _streamHelper.RemoveUnwantedChars(bigString);
                        var startIndex = _streamHelper.GetMsgHeaderStartIndex(cleandedString);

                        if (startIndex != -1)
                        {
                            Console.WriteLine("Found starting data");
                            var startTime = _streamHelper.ContainsStartTime(bigString) ? _streamHelper.GetStartTime(bigString) : DateTime.MinValue;
                            var shortenedStr = cleandedString.Substring(startIndex);
                            var measDataChunks = _measDataChunksProvider.GetMeasDataChunks(shortenedStr, startTime);
                            PrintMeasResultToConsole(measDataChunks);
                        }
                    }
                }
                else
                {
                    ShowFileNotFoundMsg();
                }
            }

            else if(CmdOptions != null &&
                    CmdOptions.LiveDataCaptureEnabled && 
                    !string.IsNullOrEmpty(CmdOptions.ComPort))
            {
                _measDataChunksProvider.MeasReceivedEvent += OnMeasReceived;

                Console.WriteLine("Press ESC key to abort");

                _measDataChunksProvider.StartReceivingMeasData(CmdOptions.ComPort);

                RunLiveCaptureLoop();

                _measDataChunksProvider.StopReceivingMeasData();
            }

            else
            {
                ShowUsageMsg(parserResults);
            }
        }

        private void RunOptionsAndReturnExitCode(Options opts)
        {
            CmdOptions = opts;
        }

        private object HandleParseError(object errs)
        {
            Console.WriteLine("Errors during command line args parsing\n\n");
            CmdOptions = null;
            return errs;
        }

        private void PrintMeasResultToConsole(IList<MeasChunk> measDataChunks)
        {
            var i = 0;
            foreach (var measDataChunk in measDataChunks)
            {
                Console.WriteLine(measDataChunk);
                i++;
            }
        }

        private void ShowUsageMsg<T>(ParserResult<T> parserResults)
        {
            var helpMgs = new HelpText();
            helpMgs.AddOptions(parserResults);
            helpMgs.Heading += "=====================================================================\n";
            helpMgs.Heading += "=== Nova SDS011 PM2.5 PM10 airpolution sensor measurments logger. ===\n";
            helpMgs.Heading += "=====================================================================\n";
            helpMgs.AddPreOptionsLine("Avialable command line options <-shortArg,--longArg>:");
            helpMgs.AddPostOptionsLine("=====================================================================");
            helpMgs.AddPostOptionsLine("======== Made by Bakterian. The app can be re-used by anyone. =======");
            helpMgs.AddPostOptionsLine("=====================================================================");
            Console.WriteLine(helpMgs);
        }

        private void ShowFileNotFoundMsg()
        {
            Console.WriteLine("Log file was not found.");
        }

        private void RunLiveCaptureLoop()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (key.Key != ConsoleKey.Escape)
            {
               key = Console.ReadKey();
            }
        }

        private void OnMeasReceived(object sender, MeasChunkEventArgs e)
        {
           Console.WriteLine(e.Chunk);
        }
    }
}
