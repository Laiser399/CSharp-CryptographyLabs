using Autofac;
using CryptographyLabs.GUI;
using CryptographyLabs.GUI.AbstractViewModels;
using CryptographyLabs.GUI.Validators;
using CryptographyLabs.GUI.ViewModels;
using FluentValidation;
using Module.RSA;

namespace CryptographyLabs;

public static class Bootstrapper
{
    public static ILifetimeScope BuildLifetimeScope()
    {
        var builder = new ContainerBuilder();

        builder
            .RegisterType<MainWindowVM>()
            .AsSelf();
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

        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true,
            RegisterRsaKeyGenerator = true,
            RegisterRsaCore = true
        });

        return builder.Build();
    }
}