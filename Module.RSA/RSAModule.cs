using Autofac;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA;

public class RSAModule : Autofac.Module
{
    public bool RegisterRsaCore { get; set; } = false;
    public bool RegisterRsaKeyGenerator { get; set; } = false;
    public bool RegisterPrimesGenerator { get; set; } = false;

    protected override void Load(ContainerBuilder builder)
    {
        if (RegisterRsaCore)
        {
            builder
                .RegisterType<RSATransformService>()
                .As<IRSATransformService>()
                .SingleInstance();
        }

        if (RegisterRsaKeyGenerator)
        {
            builder
                .RegisterType<RSAKeyPairGenerator>()
                .As<IRSAKeyPairGenerator>()
                .SingleInstance();
        }

        if (RegisterPrimesGenerator)
        {
            builder
                .RegisterType<PrimesPairGenerator>()
                .As<IPrimesPairGenerator>();
            builder
                .RegisterType<MillerRabinPrimalityTester>()
                .As<IPrimalityTester>();
            builder
                .RegisterType<RoundCountCalculator>()
                .As<IRoundCountCalculator>()
                .SingleInstance();
            builder
                .RegisterType<RandomBigIntegerGenerator>()
                .As<IRandomBigIntegerGenerator>()
                .SingleInstance();
        }

        if (RegisterRsaCore || RegisterRsaKeyGenerator || RegisterPrimesGenerator)
        {
            builder
                .RegisterType<BigIntegerCalculationService>()
                .As<IBigIntegerCalculationService>()
                .SingleInstance();
        }
    }
}