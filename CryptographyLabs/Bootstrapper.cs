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
            .RegisterGeneric(typeof(ValidationTemplate<>))
            .AsSelf();

        builder
            .RegisterType<PrimesGenerationParametersVMValidator>()
            .As<IValidator<IPrimesGenerationParametersVM>>();

        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true
        });

        return builder.Build();
    }
}