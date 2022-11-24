using Autofac;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Module.RSA;
using Util.RSA.WienerAttackTest.Entities;
using Util.RSA.WienerAttackTest.Entities.Abstract;
using Util.RSA.WienerAttackTest.Exceptions;
using Util.RSA.WienerAttackTest.Services;
using Util.RSA.WienerAttackTest.Services.Abstract;

namespace Util.RSA.WienerAttackTest;

public static class AppContainer
{
    public static IContainer Build()
    {
        var builder = new ContainerBuilder();

        RegisterConfigurations(builder);

        builder
            .RegisterType<AttackService>()
            .As<IAttackService>()
            .WithAttributeFiltering()
            .SingleInstance();

        builder
            .RegisterType<IOPathService>()
            .As<IIOPathService>()
            .SingleInstance();

        builder.RegisterModule(new RSAModule
        {
            RegisterAttackServices = true,
        });

        return builder.Build();
    }

    private static void RegisterConfigurations(ContainerBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("AppSettings.json")
            .Build();

        var applicationConfiguration = configuration
            .GetSection("Application")
            .Get<ApplicationConfiguration>();
        if (applicationConfiguration is null)
        {
            throw new ApplicationStartupException("Could not set up application configuration.");
        }

        builder
            .RegisterInstance(applicationConfiguration)
            .As<IApplicationConfiguration>();
    }
}