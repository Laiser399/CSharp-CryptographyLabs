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
            .As<IPrimesGenerationResultsVM>();
        builder
            .RegisterType<RSAKeyGenerationVM>()
            .As<IRSAKeyGenerationVM>();
        builder
            .RegisterType<RSAKeyGenerationParametersVM>()
            .As<IRSAKeyGenerationParametersVM>();
        builder
            .RegisterType<RSAKeyGenerationResultsVM>()
            .As<IRSAKeyGenerationResultsVM>();

        builder
            .RegisterGeneric(typeof(ValidationTemplate<>))
            .AsSelf();

        builder
            .RegisterType<PrimesGenerationParametersVMValidator>()
            .As<IValidator<IPrimesGenerationParametersVM>>();
        builder
            .RegisterType<RSAKeyGenerationParametersVMValidator>()
            .As<IValidator<IRSAKeyGenerationParametersVM>>();

        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true,
            RegisterRsaKeyGenerator = true
        });

        return builder.Build();
    }
}