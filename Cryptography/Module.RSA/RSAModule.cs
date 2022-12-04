using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Enums;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;

namespace Module.RSA;

public class RSAModule : Autofac.Module
{
    public bool RegisterRsaCore { get; set; }
    public bool RegisterRsaKeyGenerator { get; set; }
    public bool RegisterPrimesGenerator { get; set; }
    public bool RegisterAttackServices { get; set; }

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

        if (RegisterAttackServices)
        {
            builder
                .RegisterType<FactorizationAttackService>()
                .Keyed<IRSAAttackService>(RSAAttackType.Factorization)
                .SingleInstance();
            builder
                .RegisterType<WienerAttackService>()
                .Keyed<IRSAAttackService>(RSAAttackType.Wiener)
                .WithParameter(new TypedParameter(typeof(IRandomProvider), new RandomProvider(new Random())));
            builder
                .RegisterType<ContinuedFractionService>()
                .As<IContinuedFractionService>()
                .SingleInstance();
            builder
                .RegisterType<ConvergingFractionsService>()
                .As<IConvergingFractionsService>()
                .SingleInstance();
        }

        if (RegisterRsaCore || RegisterRsaKeyGenerator || RegisterPrimesGenerator || RegisterAttackServices)
        {
            builder
                .RegisterType<BigIntegerCalculationService>()
                .As<IBigIntegerCalculationService>()
                .SingleInstance();
        }
    }
}