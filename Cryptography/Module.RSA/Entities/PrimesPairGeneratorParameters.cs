using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class PrimesPairGeneratorParameters : IPrimesPairGeneratorParameters
{
    public int ByteCount { get; }
    public int PQDifferenceMinBitCount { get; }
    public int AddingTriesCount { get; }

    public PrimesPairGeneratorParameters(
        int byteCount,
        int pqDifferenceMinBitCount,
        int addingTriesCount)
    {
        ByteCount = byteCount;
        PQDifferenceMinBitCount = pqDifferenceMinBitCount;
        AddingTriesCount = addingTriesCount;
    }
}