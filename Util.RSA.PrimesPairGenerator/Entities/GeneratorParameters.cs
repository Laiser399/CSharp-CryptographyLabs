using Module.RSA.Entities.Abstract;

namespace Util.RSA.PrimesPairGenerator.Entities;

public class GeneratorParameters : IRandomProvider, IPrimesPairGeneratorParameters, IPrimalityTesterParameters
{
    public required Random Random { get; init; }

    public required int ByteCount { get; init; }
    public required int PQDifferenceMinBitCount { get; init; }
    public required int AddingTriesCount { get; init; }

    public required double PrimalityProbability { get; init; }
}