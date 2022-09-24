using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class MillerRabinPrimalityTester : IPrimalityTester
{
    public bool IsNotPrimary(BigInteger value)
    {
        throw new NotImplementedException();
    }
}