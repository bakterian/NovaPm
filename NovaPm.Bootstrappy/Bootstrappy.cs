using NovaPm.Infrastructure.Interfaces;
using Ninject;
using NovaPm.Control.KernelModules;

namespace NovaPm.Bootstrappy
{
    public class Bootstrappy : IBootstrappy
    {
        private readonly IKernel _kernel;

        public Bootstrappy()
        {
            _kernel = new StandardKernel();
        }

        public void RunInConsole(string[] args)
        {
            LoadCommonModule();
            LoadConsoleModule();
            _kernel.Get<IConsoleRunner>().ProcessUserInput(args);
        }

        private void LoadConsoleModule()
        {
            var consoleModule = new ConsoleModule();
            _kernel.Load(consoleModule);
        }

        private void LoadCommonModule()
        {
            var commonModule = new CommonModule();
            _kernel.Load(commonModule);
        }
    }
}
