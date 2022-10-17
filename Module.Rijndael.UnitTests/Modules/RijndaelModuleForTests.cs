using Autofac;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using Module.Rijndael.UnitTests.Entities;
using Module.Rijndael.UnitTests.Enums;

namespace Module.Rijndael.UnitTests.Modules;

public class RijndaelModuleForTests : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
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
            .RegisterType<RijndaelAddKeyService>()
            .As<IRijndaelAddKeyService>()
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