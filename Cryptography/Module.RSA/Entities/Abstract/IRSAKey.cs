using System.Numerics;

namespace Module.RSA.Entities.Abstract;

public interface IRSAKey
{
    BigInteger Exponent { get; }
    BigInteger Modulus { get; }
}