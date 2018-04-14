using NovaPm.Bootstrappy;

namespace NovaPmDataParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrappy = new Bootstrappy();
            bootstrappy.RunInConsole(args);
        }
    }
}
