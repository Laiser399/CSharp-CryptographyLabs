using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class MillerRabinPrimalityTester : IPrimalityTester
{
    private const double WrongResultProbability = 0.25;

    private readonly IRandomProvider _randomProvider;
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;
    private readonly IRandomBigIntegerGenerator _randomBigIntegerGenerator;

    private readonly int _roundCount;

    public MillerRabinPrimalityTester(
        IPrimalityTesterParameters parameters,
        IRandomProvider randomProvider,
        IRoundCountCalculator roundCountCalculator,
        IBigIntegerCalculationService bigIntegerCalculationService,
        IRandomBigIntegerGenerator randomBigIntegerGenerator)
    {
        _randomProvider = randomProvider;
        _bigIntegerCalculationService = bigIntegerCalculationService;
        _randomBigIntegerGenerator = randomBigIntegerGenerator;

        _roundCount = roundCountCalculator.GetRoundCount(parameters.PrimalityProbability, WrongResultProbability);
    }

    public bool TestIsPrime(BigInteger value)
    {
        var valueMinus1 = value - 1;
        _bigIntegerCalculationService.Factor2Out(valueMinus1, out var s, out var d);

        for (var i = 0; i < _roundCount; i++)
        {
            if (!TestIsPrimeSingleRound(value, valueMinus1, s, d))
            {
                return false;
            }
        }

        return true;
    }

    private bool TestIsPrimeSingleRound(
        BigInteger n,
        BigInteger nMinus1,
        int s,
        BigInteger d)
    {
        var a = _randomBigIntegerGenerator.Generate(2, nMinus1, _randomProvider.Random);

        if (_bigIntegerCalculationService.GreatestCommonDivisor(n, a) != 1)
        {
            return false;
        }

        var x = _bigIntegerCalculationService.BinPowMod(a, d, n);

        // Второе условие - это нулевой шаг из последующего цикла
        if (x == 1 || x == nMinus1)
        {
            return true;
        }

        for (var r = 1; r < s; ++r)
        {
            x = (x * x) % n;
            if (x == nMinus1)
            {
                return true;
            }
        }

        return false;
    }
}