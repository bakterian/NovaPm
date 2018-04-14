using CommandLine;
using NovaPm.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace NovaPm.Control.Services
{
    public class Options
    {
        [Option('l', "Logparse", Required = false, HelpText = "Parse the given log files containg hex strings.")]
        public string LogFileToBeParsed { get; set; }

        [Option('c', "capture", Required = false, HelpText = "Capture live data")]
        public bool LiveDataCaptureEnabled { get; set; }

        [Option('p', "port", Required = false, HelpText = "Serial Data COM port to be used.")]
        public string ComPort { get; set; }

        [Option('m', "mqttActive", Required = false, HelpText = "Start a MQTT server to publish the live data.")]
        public IEnumerable<string> MqttEnabled { get; set; }

        [Option('t', "timeGapMqtt", Required = false, HelpText = "Time gap beetwen the subsequent mqtt sends [s].")]
        public IEnumerable<string> MqttSendPause { get; set; }
    }

    public class CommandLineParser : ICommandLineParser
    {
        private readonly Parser _cmdParser;

        public Parser CmdParser { get { return _cmdParser; } }

        public ParserSettings Settings { get { return _cmdParser.Settings; } }

        public CommandLineParser()
        {
            _cmdParser = new Parser();
        }

        public ParserResult<T> ParseArguments<T>(IEnumerable<string> args)
        {
            return _cmdParser.ParseArguments<T>(args);
        }

        public ParserResult<T> ParseArguments<T>(Func<T> factory, IEnumerable<string> args) where T : new()
        {
            return _cmdParser.ParseArguments<T>(factory, args);
        }

        public ParserResult<object> ParseArguments(IEnumerable<string> args, params Type[] types)
        {
                return _cmdParser.ParseArguments(args, types);
        }

        public void Dispose()
        {
            _cmdParser.Dispose();
        }
    }
}
