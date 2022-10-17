using Autofac;
using Module.Core.Services;
using Module.Core.Services.Abstract;

namespace Module.Core;

public class CoreModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<XorService>()
            .As<IXorService>()
            .SingleInstance();
    }
}