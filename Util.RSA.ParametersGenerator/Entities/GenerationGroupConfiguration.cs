using Util.RSA.ParametersGenerator.Entities.Abstract;

namespace Util.RSA.ParametersGenerator.Entities;

public record GenerationGroupConfiguration(
    int ByteSize,
    int Count
) : IGenerationGroupConfiguration;