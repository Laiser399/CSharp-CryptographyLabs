using System.Numerics;
using Module.RSA.Entities.Abstract;

namespace Module.RSA.Services.Abstract;

public interface IRSAKeyPairGenerator
{
    IRSAKeyPair Generate(BigInteger p, BigInteger q);
}