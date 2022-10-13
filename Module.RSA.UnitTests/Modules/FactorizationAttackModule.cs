using Autofac;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA.UnitTests.Modules;

public class FactorizationAttackModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<FactorizationAttackService>()
            .As<IRSAAttackService>()
            .AsSelf()
            .SingleInstance();
        builder
            .RegisterType<BigIntegerCalculationService>()
            .As<IBigIntegerCalculationService>()
            .SingleInstance();
    }
}