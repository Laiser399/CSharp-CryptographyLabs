using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class MillerRabinPrimalityTester : IPrimalityTester
{
    private const double WrongResultProbability = 0.25;

    private readonly IRoundCountCalculator _roundCountCalculator;
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;
    private readonly IRandomBigIntegerGenerator _randomBigIntegerGenerator;

    private readonly Random _random;

    public MillerRabinPrimalityTester(
        IRoundCountCalculator roundCountCalculator,
        IBigIntegerCalculationService bigIntegerCalculationService,
        IRandomBigIntegerGenerator randomBigIntegerGenerator)
    {
        _roundCountCalculator = roundCountCalculator;
        _bigIntegerCalculationService = bigIntegerCalculationService;
        _randomBigIntegerGenerator = randomBigIntegerGenerator;

        _random = new Random();
    }

    public bool TestIsPrime(BigInteger value, double probability)
    {
        var roundCount = _roundCountCalculator.GetRoundCount(probability, WrongResultProbability);

        var valueMinus1 = value - 1;
        _bigIntegerCalculationService.Factor2Out(valueMinus1, out var s, out var d);

        for (var i = 0; i < roundCount; i++)
        {
            if (!TestIsPrimeSingleRound(value, valueMinus1, s, d))
            {
                return false;
            }
        }

        return true;
    }

    private bool TestIsPrimeSingleRound(
        BigInteger value,
        BigInteger valueMinus1,
        int s,
        BigInteger d)
    {
        var a = _randomBigIntegerGenerator.Generate(2, valueMinus1, _random);

        if (_bigIntegerCalculationService.GreatestCommonDivisor(value, a) != 1)
        {
            return false;
        }

        var x = _bigIntegerCalculationService.BinPowMod(a, d, value);

        // Второе условие - это нулевой шаг из последующего цикла
        if (x == 1 || x == valueMinus1)
        {
            return true;
        }

        for (var r = 1; r < s; ++r)
        {
            x = (x * x) % value;
            if (x == valueMinus1)
            {
                return true;
            }
        }

        return false;
    }
}