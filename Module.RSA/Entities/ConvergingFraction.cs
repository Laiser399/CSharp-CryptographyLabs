using System.Numerics;

namespace Module.RSA.Entities;

public struct ConvergingFraction
{
    public BigInteger P { get; }
    public BigInteger Q { get; }

    public ConvergingFraction(BigInteger p, BigInteger q)
    {
        P = p;
        Q = q;
    }
}