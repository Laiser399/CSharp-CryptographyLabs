using Autofac;
using Microsoft.Extensions.Configuration;
using Module.RSA;
using Util.RSA.ParametersGenerator.Entities;
using Util.RSA.ParametersGenerator.Entities.Abstract;
using Util.RSA.ParametersGenerator.Exceptions;
using Util.RSA.ParametersGenerator.Services;
using Util.RSA.ParametersGenerator.Services.Abstract;

namespace Util.RSA.ParametersGenerator;

public static class AppContainer
{
    public static IContainer Build()
    {
        var builder = new ContainerBuilder();

        RegisterConfigurations(builder);

        builder
            .RegisterType<RsaParametersGenerator>()
            .As<IRsaParametersGenerator>()
            .SingleInstance();

        builder
            .RegisterType<OutputPathService>()
            .As<IOutputPathService>()
            .SingleInstance();

        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true,
            RegisterRsaKeyGenerator = true
        });

        return builder.Build();
    }

    private static void RegisterConfigurations(ContainerBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("AppSettings.json")
            .Build();

        builder
            .RegisterInstance(configuration)
            .As<IConfiguration>();

        var applicationConfiguration = configuration
            .GetSection("Application")
            .Get<ApplicationConfiguration>();
        if (applicationConfiguration is null)
        {
            throw new ApplicationStartupException(
                "Could not read \"Application\" configuration section."
            );
        }

        builder
            .RegisterInstance(applicationConfiguration)
            .As<IApplicationConfiguration>();

        builder
            .RegisterType<GenerationGroupsConfiguration>()
            .As<IGenerationGroupsConfiguration>()
            .SingleInstance();
    }
}