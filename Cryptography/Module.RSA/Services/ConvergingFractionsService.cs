using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class ConvergingFractionsService : IConvergingFractionsService
{
    public IEnumerable<ConvergingFraction> EnumerateConvergingFractions(IEnumerable<BigInteger> continuedFraction)
    {
        var (p, pPrev) = ((BigInteger)1, (BigInteger)0);
        var (q, qPrev) = ((BigInteger)0, (BigInteger)1);

        foreach (var a in continuedFraction)
        {
            ValidateElement(a);

            (q, qPrev) = (a * q + qPrev, q);
            (p, pPrev) = (a * p + pPrev, p);
            yield return new ConvergingFraction(p, q);
        }
    }

    private static void ValidateElement(BigInteger a)
    {
        if (a < 0)
        {
            throw new ArgumentException($"Element of continued fraction lower than 0. Value: {a}.");
        }
    }
}