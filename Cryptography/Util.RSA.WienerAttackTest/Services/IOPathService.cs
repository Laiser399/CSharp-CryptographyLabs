using System.Text.RegularExpressions;
using Util.RSA.WienerAttackTest.Entities;
using Util.RSA.WienerAttackTest.Entities.Abstract;
using Util.RSA.WienerAttackTest.Services.Abstract;

namespace Util.RSA.WienerAttackTest.Services;

public class IOPathService : IIOPathService
{
    private static readonly Regex InputFileNameRegex = new(@"(\d+)\.json");

    private readonly IApplicationConfiguration _applicationConfiguration;

    public IOPathService(IApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    public bool TryGetInputFileMetaInfo(string inputFilePath, out InputFileMetaInfo metaInfo)
    {
        var parts = inputFilePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        if (parts.Length < 2
            || !int.TryParse(parts[^2], out var primesByteSize)
            || primesByteSize < 1)
        {
            metaInfo = null!;
            return false;
        }

        var match = InputFileNameRegex.Match(parts[^1]);
        if (!match.Success
            || !int.TryParse(match.Groups[1].Value, out var fileIndex))
        {
            metaInfo = null!;
            return false;
        }

        metaInfo = new InputFileMetaInfo(primesByteSize, fileIndex);
        return true;
    }

    public string GetOutputFilePath(int primesByteSize, int index)
    {
        return Path.Combine(_applicationConfiguration.OutputPath, primesByteSize.ToString(), $"{index}.json");
    }
}