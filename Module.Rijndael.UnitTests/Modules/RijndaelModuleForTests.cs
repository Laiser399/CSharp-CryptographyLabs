using Autofac;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
using Module.Core.Services;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using Module.Rijndael.UnitTests.Entities;

namespace Module.Rijndael.UnitTests.Modules;

public class RijndaelModuleForTests : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
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
            .Keyed<IBlockCryptoTransform>(TransformDirection.Encrypt);
        builder
            .RegisterType<RijndaelBlockDecryptTransform>()
            .Keyed<IBlockCryptoTransform>(TransformDirection.Decrypt);
        builder
            .RegisterType<XorService>()
            .As<IXorService>()
            .SingleInstance();
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
            .RegisterInstance(new GaloisFieldConfigurationForTests(0b1_0001_1011))
            .As<IGaloisFieldConfiguration>();
    }
}