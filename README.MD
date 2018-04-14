# Nova PM Measuremts Logger #

Nova SDS011 PM2.5 PM10 airpolution sensor measurments logger.
This is a Cross-Core app as it was written using .NET Core.

## Prerequisites ##
The target platform requires the dotnet core platform.
Document on how to do this on linux:
https://docs.microsoft.com/pl-pl/dotnet/core/linux-prerequisites?tabs=netcore2x

## Command Line Options ##
Avialable command line options <-shortArg,--longArg>:
l, Logparse       Parse the given log files containg hex strings.
c, capture        Capture live data
p, port           Serial Data COM port to be used.
m, mqttActive     Start a MQTT server to publish the live data.
t, timeGapMqtt    Time gap beetwen the subsequent mqtt sends [s].
help              Display more information on a specific command.
version           Display version information.


## Typical use cases  ##
dotnet -l <repoDir>\NovaPm.PublishPackage\CommandLine.dll <repoDir>\NovaPm.PublishPackage\CapturedData.log
dotnet -c -p <COM_PORT>
