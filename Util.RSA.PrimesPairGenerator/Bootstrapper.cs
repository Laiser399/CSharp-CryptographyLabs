using Autofac;
using Module.RSA;

namespace Util.RSA.PrimesPairGenerator;

public static class Bootstrapper
{
    public static ILifetimeScope BuildLifetimeScope()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new RSAModule
        {
            RegisterPrimesGenerator = true
        });

        return builder.Build();
    }
}