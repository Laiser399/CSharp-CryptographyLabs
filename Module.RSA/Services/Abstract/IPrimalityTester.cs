using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IPrimalityTester
{
    bool IsNotPrimary(BigInteger value);
}