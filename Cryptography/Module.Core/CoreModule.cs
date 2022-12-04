using Autofac;
using Module.Core.Factories;
using Module.Core.Factories.Abstract;
using Module.Core.Services;
using Module.Core.Services.Abstract;

namespace Module.Core;

public class CoreModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterGeneric(typeof(CryptoTransformFactory<>))
            .As(typeof(ICryptoTransformFactory<>));

        builder
            .RegisterType<XorService>()
            .As<IXorService>()
            .SingleInstance();
    }
}