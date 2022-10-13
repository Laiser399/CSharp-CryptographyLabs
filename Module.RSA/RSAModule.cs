using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA;

public class RSAModule : Autofac.Module
{
    public bool RegisterRsaCore { get; set; }
    public bool RegisterRsaKeyGenerator { get; set; }
    public bool RegisterPrimesGenerator { get; set; }
    public bool RegisterRsaAttackServices { get; set; }

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
                .Keyed<IRSAKeyPairGenerator>(RSAKeyPairGenerationType.Default)
                .SingleInstance();
            builder
                .RegisterType<RSAWienerAttackVulnerableKeyPairGenerator>()
                .Keyed<IRSAKeyPairGenerator>(RSAKeyPairGenerationType.WithWienerAttackVulnerability)
                .WithParameter(new TypedParameter(typeof(IRandomProvider), new RandomProvider(new Random())))
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

        if (RegisterRsaAttackServices)
        {
            builder
                .RegisterType<FactorizationAttackService>()
                .As<IRSAAttackService>()
                .SingleInstance();
        }

        if (RegisterRsaCore || RegisterRsaKeyGenerator || RegisterPrimesGenerator || RegisterRsaAttackServices)
        {
            builder
                .RegisterType<BigIntegerCalculationService>()
                .As<IBigIntegerCalculationService>()
                .SingleInstance();
        }
    }
}