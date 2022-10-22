using Autofac;
using Module.Core.Factories.Abstract;
using Module.DES.Cryptography;
using Module.DES.Entities.Abstract;
using Module.DES.Enums;
using Module.DES.Factories;
using Module.DES.Factories.Abstract;
using Module.DES.Services;
using Module.DES.Services.Abstract;

namespace Module.DES;

public class DesModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<DesBlockEncryptTransform>()
            .AsSelf();
        builder
            .RegisterType<DesBlockDecryptTransform>()
            .AsSelf();
        
        RegisterFactories(builder);
        RegisterServices(builder);
        RegisterPermutationServices(builder);
    }

    private static void RegisterFactories(ContainerBuilder builder)
    {
        builder
            .RegisterType<DesBlockCryptoTransformFactory>()
            .As<IBlockCryptoTransformFactory<IDesParameters>>()
            .SingleInstance();
        builder
            .RegisterType<DesKeyFactory>()
            .As<IDesKeyFactory>()
            .SingleInstance();
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        builder
            .RegisterType<DesRoundKeysGenerator>()
            .As<IDesRoundKeysGenerator>()
            .SingleInstance();
        builder
            .RegisterType<BitOperationsService>()
            .As<IBitOperationsService>()
            .SingleInstance();
        builder
            .RegisterType<BitPermutationService>()
            .As<IBitPermutationService>()
            .SingleInstance();
        builder
            .RegisterType<FeistelFunctionService>()
            .As<IFeistelFunctionService>()
            .SingleInstance();
        builder
            .RegisterType<DesExpandFunction>()
            .As<IDesExpandFunction>()
            .SingleInstance();
        builder
            .RegisterType<DesSubstitutionService>()
            .As<IDesSubstitutionService>()
            .SingleInstance();
    }

    private static void RegisterPermutationServices(ContainerBuilder builder)
    {
        builder
            .RegisterType<KeyInitialPermutationService>()
            .Keyed<IUInt64BitPermutationService>(PermutationType.KeyInitial)
            .SingleInstance();
        builder
            .RegisterType<KeyFinalPermutationService>()
            .Keyed<IUInt64BitPermutationService>(PermutationType.KeyFinal)
            .SingleInstance();
        builder
            .RegisterType<FeistelFunctionPermutationService>()
            .Keyed<IUInt32BitPermutationService>(PermutationType.Feistel)
            .SingleInstance();
        builder
            .RegisterType<DesInitialPermutationService>()
            .Keyed<IUInt64BitPermutationService>(PermutationType.Initial)
            .SingleInstance();
        builder
            .RegisterType<DesFinalPermutationService>()
            .Keyed<IUInt64BitPermutationService>(PermutationType.Final)
            .SingleInstance();
    }
}