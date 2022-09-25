using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IRandomBigIntegerGenerator
{
    /// <summary>
    /// Предел генерации [<paramref name="min"/>; <paramref name="max"/>)
    /// </summary>
    BigInteger Generate(BigInteger min, BigInteger max, Random random);

    /// <summary>
    /// Предел генерации [0; <paramref name="max"/>)
    /// </summary>
    /// <param name="max"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    BigInteger Generate(BigInteger max, Random random);
}