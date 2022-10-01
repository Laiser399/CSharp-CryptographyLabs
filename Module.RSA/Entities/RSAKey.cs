using System.Numerics;
using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class RSAKey : IRSAKey
{
    public BigInteger Exponent { get; }
    public BigInteger Modulus { get; }

    public RSAKey(BigInteger exponent, BigInteger modulus)
    {
        Exponent = exponent;
        Modulus = modulus;
    }
}