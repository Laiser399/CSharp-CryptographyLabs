using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
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
        builder
            .RegisterInstance(new RandomProvider(new Random(123)))
            .As<IRandomProvider>();
        builder
            .RegisterType<BigIntegerCalculationService>()
            .As<IBigIntegerCalculationService>()
            .SingleInstance();
        builder
            .RegisterType<ContinuedFractionService>()
            .As<IContinuedFractionService>()
            .SingleInstance();
        builder
            .RegisterType<ConvergingFractionsService>()
            .As<IConvergingFractionsService>()
            .SingleInstance();
    }
}