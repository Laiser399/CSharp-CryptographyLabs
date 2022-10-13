using Autofac;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA.UnitTests.Modules;

public class WienerAttackModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<WienerAttackService>()
            .As<IRSAAttackService>()
            .AsSelf();
    }
}