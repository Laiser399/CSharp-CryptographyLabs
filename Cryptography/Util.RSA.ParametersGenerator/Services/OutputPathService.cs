using Util.RSA.ParametersGenerator.Entities.Abstract;
using Util.RSA.ParametersGenerator.Services.Abstract;

namespace Util.RSA.ParametersGenerator.Services;

public class OutputPathService : IOutputPathService
{
    private readonly IApplicationConfiguration _applicationConfiguration;

    public OutputPathService(IApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    public string GetOutputFilePath(int byteSize, int index)
    {
        return Path.Combine(_applicationConfiguration.OutputPath, byteSize.ToString(), $"{index}.json");
    }
}