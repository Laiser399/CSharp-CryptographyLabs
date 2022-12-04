using System.Diagnostics;
using System.Numerics;
using Autofac.Features.AttributeFilters;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Enums;
using Module.RSA.Exceptions;
using Module.RSA.Extensions;
using Module.RSA.Services.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Util.RSA.WienerAttackTest.Entities;
using Util.RSA.WienerAttackTest.Entities.Abstract;
using Util.RSA.WienerAttackTest.Exceptions;
using Util.RSA.WienerAttackTest.Services.Abstract;

namespace Util.RSA.WienerAttackTest.Services;

public class AttackService : IAttackService
{
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IIOPathService _ioPathService;
    private readonly Func<IWienerAttackStatisticsCollector, IRSAAttackService> _attackServiceFactory;

    public AttackService(
        IApplicationConfiguration applicationConfiguration,
        IIOPathService ioPathService,
        [KeyFilter(RSAAttackType.Wiener)]
        Func<IWienerAttackStatisticsCollector, IRSAAttackService> attackServiceFactory)
    {
        _applicationConfiguration = applicationConfiguration;
        _ioPathService = ioPathService;
        _attackServiceFactory = attackServiceFactory;
    }

    public async Task PerformAttacksAsync()
    {
        foreach (var inputFilePath in EnumerateInputFiles())
        {
            if (!_ioPathService.TryGetInputFileMetaInfo(inputFilePath, out var metaInfo))
            {
                throw new PerformAttackException($"Could not parse path of input file \"{inputFilePath}\".");
            }

            var outputFilePath = _ioPathService.GetOutputFilePath(metaInfo.PrimesByteSize, metaInfo.FileIndex);

            if (File.Exists(outputFilePath)
                && !_applicationConfiguration.RewriteExistingFiles)
            {
                continue;
            }

            var keyPair = ReadKeyPair(inputFilePath);

            var attackResult = await PerformAttackAsync(keyPair);

            SaveResult(outputFilePath, attackResult);
        }
    }

    private IEnumerable<string> EnumerateInputFiles()
    {
        return EnumerateFilesRecursively(_applicationConfiguration.InputPath);
    }

    private static IEnumerable<string> EnumerateFilesRecursively(string directoryPath)
    {
        foreach (var subDirectoryPath in Directory.GetDirectories(directoryPath))
        {
            foreach (var filePath in EnumerateFilesRecursively(subDirectoryPath))
            {
                yield return filePath;
            }
        }

        foreach (var filePath in Directory.GetFiles(directoryPath))
        {
            yield return filePath;
        }
    }

    private static IRSAKeyPair ReadKeyPair(string inputFilePath)
    {
        var fileContent = File.ReadAllText(inputFilePath);
        var jObject = JObject.Parse(fileContent);

        var publicExponent = jObject["PublicExponent"]?.ToObject<BigInteger>()
                             ?? throw new PerformAttackException(
                                 $"Could now read value of \"PublicExponent\" from file \"{inputFilePath}\"."
                             );
        var privateExponent = jObject["PrivateExponent"]?.ToObject<BigInteger>()
                              ?? throw new PerformAttackException(
                                  $"Could now read value of \"PrivateExponent\" from file \"{inputFilePath}\"."
                              );
        var modulus = jObject["Modulus"]?.ToObject<BigInteger>()
                      ?? throw new PerformAttackException(
                          $"Could now read value of \"Modulus\" from file \"{inputFilePath}\"."
                      );

        return new RSAKeyPair(
            new RSAKey(publicExponent, modulus),
            new RSAKey(privateExponent, modulus)
        );
    }

    private async Task<AttackResult> PerformAttackAsync(IRSAKeyPair keyPair)
    {
        var statistics = new WienerAttackStatistics();
        var attackService = _attackServiceFactory(statistics);

        string? errorMessage = null;
        var isUnexpectedResult = false;

        var tokenSource = new CancellationTokenSource(_applicationConfiguration.AttackTimeout);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var actualPrivateExponent = await attackService.AttackAsync(keyPair.Public, tokenSource.Token);
            isUnexpectedResult = keyPair.Private.Exponent != actualPrivateExponent;
        }
        catch (CryptographyAttackException e)
        {
            errorMessage = e.Message;
        }
        catch (OperationCanceledException)
        {
            errorMessage = "Timeout exceeded for RSA attack.";
        }
        finally
        {
            stopwatch.Stop();
        }

        return new AttackResult(
            stopwatch.Elapsed,
            statistics.ExponentsCheckedCount,
            errorMessage,
            isUnexpectedResult
        );
    }

    private static void SaveResult(string outputFilePath, AttackResult attackResult)
    {
        var serialized = JsonConvert.SerializeObject(attackResult);

        var directoryName = Path.GetDirectoryName(outputFilePath) ?? string.Empty;
        Directory.CreateDirectory(directoryName);
        File.WriteAllText(outputFilePath, serialized);
    }
}