using Autofac;
using Autofac.Features.AttributeFilters;
using Module.RSA;
using Util.RSA.WienerAttackTest.Services;
using Util.RSA.WienerAttackTest.Services.Abstract;

namespace Util.RSA.WienerAttackTest;

public static class AppContainer
{
    public static IContainer Build()
    {
        var builder = new ContainerBuilder();

        builder
            .RegisterType<WienerAttackTestService>()
            .As<IWienerAttackTestService>()
            .WithAttributeFiltering();

        builder.RegisterModule(new RSAModule
        {
            RegisterRsaCore = true,
            RegisterPrimesGenerator = true,
            RegisterRsaKeyGenerator = true,
            RegisterAttackServices = true,
        });

        return builder.Build();
    }
}