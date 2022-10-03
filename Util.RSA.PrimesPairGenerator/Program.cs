// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;
using Util.RSA.PrimesPairGenerator;

var lifetimeScope = Bootstrapper.BuildLifetimeScope();

var parameters = new PrimesPairGeneratorCombinedParameters(
    new Random(),
    256,
    256 * 8 - 1,
    100,
    0.995
);

GeneratePQ(lifetimeScope, parameters, out var p, out var q);

const string saveDirectoryName = "PrimePairs";

if (!Directory.Exists(saveDirectoryName))
{
    Directory.CreateDirectory(saveDirectoryName);
}

var timeStr = DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss.fff");
File.WriteAllText(Path.Combine(saveDirectoryName, $"{timeStr} p.txt"), p.ToString());
File.WriteAllText(Path.Combine(saveDirectoryName, $"{timeStr} q.txt"), q.ToString());
Console.WriteLine("Result saved");


static void GeneratePQ(
    ILifetimeScope lifetimeScope,
    PrimesPairGeneratorCombinedParameters parameters,
    out BigInteger p,
    out BigInteger q)
{
    using var scope = RegisterParameters(lifetimeScope, parameters);
    var generator = scope.Resolve<IPrimesPairGenerator>();

    Console.WriteLine("Start generation...");
    var stopwatch = Stopwatch.StartNew();
    generator.Generate(out p, out q);
    stopwatch.Stop();
    Console.WriteLine($"Generation done in {stopwatch.Elapsed}");
}


static ILifetimeScope RegisterParameters(
    ILifetimeScope scope,
    PrimesPairGeneratorCombinedParameters parameters)
{
    return scope.BeginLifetimeScope(builder =>
    {
        builder
            .Register(_ => parameters)
            .As<IRandomProvider>()
            .As<IPrimesPairGeneratorParameters>()
            .As<IPrimalityTesterParameters>()
            .SingleInstance();
    });
}