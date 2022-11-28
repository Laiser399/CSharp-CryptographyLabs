using Autofac;
using CryptographyLabs.GUI;
using CryptographyLabs.GUI.AbstractViewModels;
using CryptographyLabs.GUI.Validators;
using CryptographyLabs.GUI.ViewModels;
using FluentValidation;
using Module.Core;
using Module.DES;
using Module.PermutationNetwork;
using Module.Rijndael;
using Module.RSA;

namespace CryptographyLabs;

public static class AppContainer
{
    public static IContainer Build()
    {
        var builder = new ContainerBuilder();

        builder
            .RegisterType<MainWindowVM>()
            .AsSelf();
        builder
            .RegisterType<GaloisFieldElementRepresentationVM>()
            .As<IGaloisFieldElementRepresentationVM>();
        builder
            .RegisterType<BinaryPolynomialMultiplicationVM>()
            .As<IBinaryPolynomialMultiplicationVM>();
        builder
            .RegisterType<PrimesGenerationVM>()
            .As<IPrimesGenerationVM>();
        builder
            .RegisterType<PrimesGenerationParametersVM>()
            .As<IPrimesGenerationParametersVM>();
        builder
            .RegisterType<PrimesGenerationResultsVM>()
            .As<IPrimesGenerationResultsVM>()
            .InstancePerMatchingLifetimeScope(nameof(MainWindowVM));
        builder
            .RegisterType<RSAKeyGenerationVM>()
            .As<IRSAKeyGenerationVM>();
        builder
            .RegisterType<RSAKeyGenerationParametersVM>()
            .As<IRSAKeyGenerationParametersVM>();
        builder
            .RegisterType<RSAKeyGenerationResultsVM>()
            .As<IRSAKeyGenerationResultsVM>()
            .InstancePerMatchingLifetimeScope(nameof(MainWindowVM));
        builder
            .RegisterType<RSATransformationVM>()
            .As<IRSATransformationVM>();
        builder
            .RegisterType<RSATransformationParametersVM>()
            .As<IRSATransformationParametersVM>();
        builder
            .RegisterType<RSAAttackVM>()
            .As<IRSAAttackVM>();
        builder
            .RegisterType<RSAAttackParametersVM>()
            .As<IRSAAttackParametersVM>();
        builder
            .RegisterType<RSAAttackResultsVM>()
            .As<IRSAAttackResultsVM>();

        builder
            .RegisterGeneric(typeof(ValidationTemplate<>))
            .AsSelf();

        builder
            .RegisterType<PrimesGenerationParametersVMValidator>()
            .As<IValidator<IPrimesGenerationParametersVM>>();
        builder
            .RegisterType<RSAKeyGenerationParametersVMValidator>()
            .As<IValidator<IRSAKeyGenerationParametersVM>>();
        builder
            .RegisterType<RSATransformationParametersVMValidator>()
            .As<IValidator<IRSATransformationParametersVM>>();
        builder
            .RegisterType<RSAAttackParametersVMValidator>()
            .As<IValidator<IRSAAttackParametersVM>>();

        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true,
            RegisterRsaKeyGenerator = true,
            RegisterRsaCore = true,
            RegisterAttackServices = true
        });

        builder.RegisterModule(new RijndaelModule
        {
            UseDefaultGaloisFieldConfiguration = true
        });

        builder.RegisterModule<DesModule>();

        builder.RegisterModule<PermutationNetworkModule>();

        builder.RegisterModule<CoreModule>();

        return builder.Build();
    }
}