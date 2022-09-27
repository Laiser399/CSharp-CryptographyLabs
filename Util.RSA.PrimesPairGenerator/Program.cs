// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using Autofac;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;
using Util.RSA.PrimesPairGenerator;
using Util.RSA.PrimesPairGenerator.Entities;

var lifetimeScope = Bootstrapper.BuildLifetimeScope();

var parameters = new GeneratorParameters
{
    Random = new Random(),
    ByteCount = 256,
    PQDifferenceMinBitCount = 256 * 8 - 1,
    AddingTriesCount = 100,
    PrimalityProbability = 0.995
};

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


static void GeneratePQ(ILifetimeScope lifetimeScope, GeneratorParameters parameters, out BigInteger p, out BigInteger q)
{
    using var scope = RegisterParameters(lifetimeScope, parameters);
    var generator = scope.Resolve<IPrimesPairGenerator>();

    Console.WriteLine("Start generation...");
    var stopwatch = Stopwatch.StartNew();
    generator.Generate(out p, out q);
    stopwatch.Stop();
    Console.WriteLine($"Generation done in {stopwatch.Elapsed}");
}


static ILifetimeScope RegisterParameters(ILifetimeScope scope, GeneratorParameters parameters)
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