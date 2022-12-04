using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Extensions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSAWienerAttackVulnerableKeyPairGenerator : IRSAKeyPairGenerator
{
    private readonly IRandomProvider _randomProvider;
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;
    private readonly IRandomBigIntegerGenerator _randomBigIntegerGenerator;

    public RSAWienerAttackVulnerableKeyPairGenerator(
        IRandomProvider randomProvider,
        IBigIntegerCalculationService bigIntegerCalculationService,
        IRandomBigIntegerGenerator randomBigIntegerGenerator)
    {
        _randomProvider = randomProvider;
        _bigIntegerCalculationService = bigIntegerCalculationService;
        _randomBigIntegerGenerator = randomBigIntegerGenerator;
    }

    public IRSAKeyPair Generate(BigInteger p, BigInteger q)
    {
        BigInteger e, d;
        var n = p * q;
        var phiN = (p - 1) * (q - 1);
        var wienerAttackVulnerabilityThreshold = _bigIntegerCalculationService.FourthRoot(n) / 3;

        while (true)
        {
            d = GetStartPrivateExponent(wienerAttackVulnerabilityThreshold);

            while (d > 1 && _bigIntegerCalculationService.GreatestCommonDivisor(d, phiN, out e, out _) != 1)
            {
                d -= 2;
            }

            if (d > 1)
            {
                break;
            }
        }

        e = e.NormalizedMod(phiN);

        return new RSAKeyPair(
            new RSAKey(e, n),
            new RSAKey(d, n)
        );
    }

    private BigInteger GetStartPrivateExponent(BigInteger wienerAttackVulnerabilityThreshold)
    {
        var d = _randomBigIntegerGenerator.Generate(
            3,
            wienerAttackVulnerabilityThreshold,
            _randomProvider.Random
        );
        d |= 1;

        return d;
    }
}