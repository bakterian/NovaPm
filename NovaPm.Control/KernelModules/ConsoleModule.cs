using Ninject.Modules;
using NovaPm.Infrastructure.Interfaces;
using NovaPm.Control.Services;

namespace NovaPm.Control.KernelModules
{
    public class ConsoleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConsoleRunner>().To<ConsoleRunner>().InSingletonScope();
            Bind<IUserInputManager>().To<UserInputManager>();
        }
    }
}
