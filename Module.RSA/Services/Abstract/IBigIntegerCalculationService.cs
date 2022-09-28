using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IBigIntegerCalculationService
{
    /// <summary>
    /// Быстрое возведение в степень по модулю <paramref name="mod"/>
    /// </summary>
    BigInteger BinPowMod(BigInteger value, BigInteger pow, BigInteger mod);

    /// <summary>
    /// Вычисляет значения такие, что<br/>
    ///     <paramref name="value"/> = 2^<paramref name="exponent2"/> * <paramref name="remainder"/>,<br/>
    /// где <paramref name="remainder"/> - нечетное
    /// </summary>
    /// <param name="value"></param>
    /// <param name="exponent2"></param>
    /// <param name="remainder"></param>
    void Factor2Out(BigInteger value, out int exponent2, out BigInteger remainder);

    BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b);

    /// <summary>
    /// Помимо НОД вычисляет коэффициенты <paramref name="x"/>, <paramref name="y"/> такие, что<br/>
    /// x*a + y*b = gcd(a, b)
    /// </summary>
    BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y);

    /// <summary>
    /// Корень четвертой степени
    /// </summary>
    BigInteger FourthRoot(BigInteger value);
}