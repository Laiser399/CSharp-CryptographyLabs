﻿using Autofac;
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

        if (RegisterRsaAttackServices)
        {
            builder
                .RegisterType<RSAFactorizationAttackService>()
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