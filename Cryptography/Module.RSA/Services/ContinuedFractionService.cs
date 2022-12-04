using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class ContinuedFractionService : IContinuedFractionService
{
    public IEnumerable<BigInteger> EnumerateContinuedFraction(BigInteger numerator, BigInteger denominator)
    {
        ValidateArguments(numerator, denominator);

        return Enumerate(numerator, denominator);
    }

    private static IEnumerable<BigInteger> Enumerate(BigInteger numerator, BigInteger denominator)
    {
        while (denominator > 0)
        {
            yield return numerator / denominator;
            (numerator, denominator) = (denominator, numerator % denominator);
        }
    }

    private static void ValidateArguments(BigInteger numerator, BigInteger denominator)
    {
        if (numerator < 0)
        {
            throw new ArgumentException("Numerator is negative.", nameof(numerator));
        }

        if (denominator <= 0)
        {
            throw new ArgumentException("Denominator lower or equal 0.", nameof(denominator));
        }
    }
}