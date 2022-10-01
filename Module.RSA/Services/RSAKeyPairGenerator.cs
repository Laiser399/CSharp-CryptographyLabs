using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSAKeyPairGenerator : IRSAKeyPairGenerator
{
    private static readonly BigInteger StartEncryptionExponent = 65537;

    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;

    public RSAKeyPairGenerator(IBigIntegerCalculationService bigIntegerCalculationService)
    {
        _bigIntegerCalculationService = bigIntegerCalculationService;
    }

    public IRSAKeyPair Generate(BigInteger p, BigInteger q)
    {
        var e = StartEncryptionExponent;
        BigInteger d;
        var n = p * q;
        var phiN = (p - 1) * (q - 1);
        var wienerAttackVulnerabilityThreshold = _bigIntegerCalculationService.FourthRoot(n) / 3;

        while (true)
        {
            while (_bigIntegerCalculationService.GreatestCommonDivisor(e, phiN, out d, out _) != 1)
            {
                e += 2;
            }

            d %= phiN;
            if (d < 0)
            {
                d += phiN;
            }

            if (d > wienerAttackVulnerabilityThreshold)
            {
                break;
            }

            e += 2;
        }

        return new RSAKeyPair(
            new RSAKey(e, n),
            new RSAKey(d, n)
        );
    }
}