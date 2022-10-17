using Autofac;
using Module.Core;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
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
            .RegisterType<RijndaelCryptoTransformFactory>()
            .As<IRijndaelCryptoTransformFactory>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelKeyFactory>()
            .As<IRijndaelKeyFactory>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelParametersFactory>()
            .As<IRijndaelParametersFactory>()
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
            .RegisterType<GaloisFieldConfigurationFactory>()
            .As<IGaloisFieldConfigurationFactory>()
            .SingleInstance();
        builder
            .RegisterType<GaloisFieldService>()
            .As<IGaloisFieldService>()
            .SingleInstance();
        builder
            .Register(x => x.Resolve<IGaloisFieldConfigurationFactory>().Default)
            .As<IGaloisFieldConfiguration>()
            .SingleInstance();
    }
}