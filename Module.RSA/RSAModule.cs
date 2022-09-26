using Autofac;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA;

public class RSAModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<PrimesPairGenerator>()
            .As<IPrimesPairGenerator>();
        builder
            .RegisterType<MillerRabinPrimalityTester>()
            .As<IPrimalityTester>()
            .SingleInstance();
        builder
            .RegisterType<RoundCountCalculator>()
            .As<IRoundCountCalculator>()
            .SingleInstance();
        builder
            .RegisterType<BigIntegerCalculationService>()
            .As<IBigIntegerCalculationService>()
            .SingleInstance();
        builder
            .RegisterType<RandomBigIntegerGenerator>()
            .As<IRandomBigIntegerGenerator>()
            .SingleInstance();
    }
}