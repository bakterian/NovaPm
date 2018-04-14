using Ninject.Modules;
using NovaPm.Control.Services;
using NovaPm.Infrastructure.Interfaces;

namespace NovaPm.Control.KernelModules
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IParameterStore>().To<ParameterStore>();
            Bind<IMeasChunkCreator>().To<MeasChunkCreator>();
            Bind<IMeasDataChunksProvider>().To<MeasDataChunksProvider>();
            Bind<IStreamHelper>().To<StreamHelper>();
            Bind<ICommandLineParser>().To<CommandLineParser>();
            Bind<IMySerialReader>().To<MySerialReader>();
        }
    }
}
