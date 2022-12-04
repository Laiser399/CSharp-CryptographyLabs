using Autofac;
using Autofac.Features.Indexed;
using Module.Core.Converters;
using Module.RSA;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;
using Newtonsoft.Json;
using Util.RSA.ParametersGenerator.Entities;
using Util.RSA.ParametersGenerator.Entities.Abstract;
using Util.RSA.ParametersGenerator.Services.Abstract;

namespace Util.RSA.ParametersGenerator.Services;

public class RsaParametersGenerator : IRsaParametersGenerator
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IGenerationGroupsConfiguration _generationGroupsConfiguration;
    private readonly IIndex<RSAKeyPairGenerationType, IRSAKeyPairGenerator> _rsaKeyPairGenerators;
    private readonly IOutputPathService _outputPathService;

    public RsaParametersGenerator(
        ILifetimeScope lifetimeScope,
        IApplicationConfiguration applicationConfiguration,
        IGenerationGroupsConfiguration generationGroupsConfiguration,
        IIndex<RSAKeyPairGenerationType, IRSAKeyPairGenerator> rsaKeyPairGenerators,
        IOutputPathService outputPathService)
    {
        _lifetimeScope = lifetimeScope;
        _applicationConfiguration = applicationConfiguration;
        _generationGroupsConfiguration = generationGroupsConfiguration;
        _rsaKeyPairGenerators = rsaKeyPairGenerators;
        _outputPathService = outputPathService;
    }

    public void GenerateAndSave()
    {
        foreach (var (primesByteSize, count) in _generationGroupsConfiguration.Groups)
        {
            for (var i = 0; i < count; i++)
            {
                GenerateAndSave(primesByteSize, i);
            }
        }
    }

    private void GenerateAndSave(int primesByteSize, int index)
    {
        var outputFilePath = _outputPathService.GetOutputFilePath(primesByteSize, index);

        if (File.Exists(outputFilePath)
            && !_applicationConfiguration.RewriteExistingFiles)
        {
            return;
        }

        var rsaParameters = GenerateRsaParameters(primesByteSize);

        SaveRsaParameters(outputFilePath, rsaParameters);
    }

    private RsaParameters GenerateRsaParameters(int primesByteSize)
    {
        var generationParameters = CreateGenerationParameters(primesByteSize);
        using var lifetimeScope = RegisterGenerationParameters(generationParameters);

        var primesPairGenerator = lifetimeScope.Resolve<IPrimesPairGenerator>();
        primesPairGenerator.Generate(out var p, out var q);

        var rsaKeyPairGenerator = _rsaKeyPairGenerators[_applicationConfiguration.RSAKeyPairGenerationType];
        var keyPair = rsaKeyPairGenerator.Generate(p, q);

        return new RsaParameters(
            p,
            q,
            keyPair.Public.Exponent,
            keyPair.Private.Exponent,
            keyPair.Public.Modulus
        );
    }

    private PrimesPairGeneratorCombinedParameters CreateGenerationParameters(int primesByteSize)
    {
        return new PrimesPairGeneratorCombinedParameters(
            _applicationConfiguration.RandomSeed is not null
                ? new Random(_applicationConfiguration.RandomSeed.Value)
                : new Random(),
            primesByteSize,
            primesByteSize * 8 - 1,
            _applicationConfiguration.AddingTriesCount,
            _applicationConfiguration.PrimalityProbability
        );
    }

    private ILifetimeScope RegisterGenerationParameters(PrimesPairGeneratorCombinedParameters parameters)
    {
        return _lifetimeScope.BeginLifetimeScope(builder =>
        {
            builder
                .Register(_ => parameters)
                .As<IRandomProvider>()
                .As<IPrimesPairGeneratorParameters>()
                .As<IPrimalityTesterParameters>()
                .SingleInstance();
        });
    }

    private static void SaveRsaParameters(string outputFilePath, RsaParameters rsaParameters)
    {
        var serialized = JsonConvert.SerializeObject(rsaParameters, new BigIntegerConverter());

        var directoryName = Path.GetDirectoryName(outputFilePath) ?? string.Empty;
        Directory.CreateDirectory(directoryName);

        File.WriteAllText(outputFilePath, serialized);
    }
}