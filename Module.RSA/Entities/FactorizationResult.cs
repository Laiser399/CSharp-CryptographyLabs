using System.Numerics;

namespace Module.RSA.Entities;

public class FactorizationResult
{
    public BigInteger LowerFactor { get; }
    public BigInteger HigherFactor { get; }

    public FactorizationResult(BigInteger lowerFactor, BigInteger higherFactor)
    {
        LowerFactor = lowerFactor;
        HigherFactor = higherFactor;
    }
}