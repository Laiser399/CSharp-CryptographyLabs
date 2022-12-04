using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class PrimesPairGeneratorCombinedParameters :
    IRandomProvider,
    IPrimesPairGeneratorParameters,
    IPrimalityTesterParameters
{
    public Random Random { get; }

    public int ByteCount { get; }
    public int PQDifferenceMinBitCount { get; }
    public int AddingTriesCount { get; }

    public double PrimalityProbability { get; }

    public PrimesPairGeneratorCombinedParameters(
        Random random,
        int byteCount,
        int pqDifferenceMinBitCount,
        int addingTriesCount,
        double primalityProbability)
    {
        Random = random;
        ByteCount = byteCount;
        PQDifferenceMinBitCount = pqDifferenceMinBitCount;
        AddingTriesCount = addingTriesCount;
        PrimalityProbability = primalityProbability;
    }
}