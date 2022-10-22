using Autofac;
using Module.Core;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Rijndael.Cryptography;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.UnitTests.Modules;

public class RijndaelModuleForTests : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<CoreModule>();

        builder
            .RegisterType<RijndaelBlockCryptoTransformFactory>()
            .As<IBlockCryptoTransformFactory<IRijndaelParameters>>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelKeyFactory>()
            .As<IRijndaelKeyFactory>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelBlockCryptoTransformParametersFactory>()
            .As<IRijndaelBlockCryptoTransformParametersFactory>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelExtendedKeyGenerator>()
            .As<IRijndaelExtendedKeyGenerator>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelRoundCountCalculator>()
            .As<IRijndaelRoundCountCalculator>()
            .SingleInstance();

        builder
            .RegisterType<RijndaelBlockEncryptTransform>()
            .AsSelf()
            .Keyed<IBlockCryptoTransform>(TransformDirection.Encrypt);
        builder
            .RegisterType<RijndaelBlockDecryptTransform>()
            .AsSelf()
            .Keyed<IBlockCryptoTransform>(TransformDirection.Decrypt);
        builder
            .RegisterType<RijndaelSubstitutionService>()
            .As<IRijndaelSubstitutionService>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelShiftRowsService>()
            .As<IRijndaelShiftRowsService>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelMixColumnsService>()
            .As<IRijndaelMixColumnsService>()
            .SingleInstance();
        builder
            .RegisterType<GaloisFieldCalculationService>()
            .As<IGaloisFieldCalculationService>()
            .SingleInstance();
        builder
            .RegisterInstance(GaloisFieldConfigurationFactory.DefaultConfiguration)
            .As<IGaloisFieldConfiguration>();
    }
}