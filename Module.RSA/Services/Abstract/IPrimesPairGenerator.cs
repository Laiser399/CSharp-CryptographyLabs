using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IPrimesPairGenerator
{
    void Generate(out BigInteger p, out BigInteger q);
    Task<(BigInteger p, BigInteger q)> GenerateAsync();
}