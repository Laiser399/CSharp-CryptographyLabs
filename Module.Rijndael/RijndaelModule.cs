using Autofac;
using Module.Core.Factories.Abstract;
using Module.Rijndael.Cryptography;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael;

public class RijndaelModule : Autofac.Module
{
    public bool UseDefaultGaloisFieldConfiguration { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<RijndaelBlockEncryptTransform>()
            .AsSelf();
        builder
            .RegisterType<RijndaelBlockDecryptTransform>()
            .AsSelf();

        RegisterFactories(builder);
        RegisterServices(builder);
    }

    private void RegisterFactories(ContainerBuilder builder)
    {
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

        if (!UseDefaultGaloisFieldConfiguration)
        {
            builder
                .RegisterType<GaloisFieldConfigurationFactory>()
                .As<IGaloisFieldConfigurationFactory>()
                .SingleInstance();
        }
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        builder
            .RegisterType<RijndaelExtendedKeyGenerator>()
            .As<IRijndaelExtendedKeyGenerator>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelRoundCountCalculator>()
            .As<IRijndaelRoundCountCalculator>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelSubstitutionService>()
            .As<IRijndaelSubstitutionService>()
            .SingleInstance();
        builder
            .RegisterType<RijndaelShiftRowsService>()
            .As<IRijndaelShiftRowsService>()
            .SingleInstance();

        if (UseDefaultGaloisFieldConfiguration)
        {
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
        else
        {
            builder
                .RegisterType<RijndaelMixColumnsService>()
                .As<IRijndaelMixColumnsService>();
            builder
                .RegisterType<GaloisFieldCalculationService>()
                .As<IGaloisFieldCalculationService>();
        }

        builder
            .RegisterType<GaloisFieldService>()
            .As<IGaloisFieldService>()
            .SingleInstance();

        builder
            .RegisterType<BinaryPolynomialRepresentationService>()
            .As<IBinaryPolynomialRepresentationService>()
            .SingleInstance();

        builder
            .RegisterType<BinaryPolynomialsCalculationService>()
            .As<IBinaryPolynomialsCalculationService>()
            .SingleInstance();
    }
}