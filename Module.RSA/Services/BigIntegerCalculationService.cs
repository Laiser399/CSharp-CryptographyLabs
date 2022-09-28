using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class BigIntegerCalculationService : IBigIntegerCalculationService
{
    public BigInteger BinPowMod(BigInteger value, BigInteger pow, BigInteger mod)
    {
        if (value == 0 && pow == 0)
            throw new ArgumentException("value = 0 and pow = 0. That's all.");
        if (pow < 0)
            throw new ArgumentOutOfRangeException(nameof(pow), "Pow must be >= 0.");
        if (mod < 1)
            throw new ArgumentOutOfRangeException(nameof(mod), "Mod must be > 0.");

        var multiplier = value % mod;
        if (multiplier < 0)
        {
            multiplier += mod;
        }

        BigInteger result = 1;
        while (pow > 0)
        {
            if ((pow & 1) == 1)
            {
                result = (result * multiplier) % mod;
            }

            multiplier = (multiplier * multiplier) % mod;
            pow >>= 1;
        }

        return result;
    }

    public void Factor2Out(BigInteger value, out int exponent2, out BigInteger remainder)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Value must be positive.", nameof(value));
        }

        exponent2 = 0;
        remainder = value;
        while ((remainder & 1) == 0)
        {
            ++exponent2;
            remainder >>= 1;
        }
    }

    public BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        if (a < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), "Value must be >= 0.");
        }

        if (b < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Value must be >= 0.");
        }

        while (b > 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }

    public BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), "Value must be >= 0.");
        }

        if (b < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Value must be >= 0.");
        }

        // Считаем их предыдущими. На каждом шаге принимают такие значения, что a = x * a0 + y * b0.
        (x, y) = (1, 0);
        // Считаем их текущими. На каждом шаге принимают такие значения, что b = xCurrent * a0 + yCurrent * b0.
        var (xCurrent, yCurrent) = ((BigInteger)0, (BigInteger)1);
        while (b > 0)
        {
            var q = a / b;
            (a, b) = (b, a % b);
            (xCurrent, x) = (x - q * xCurrent, xCurrent);
            (yCurrent, y) = (y - q * yCurrent, yCurrent);
        }

        return a;
    }

    public BigInteger FourthRoot(BigInteger value)
    {
        throw new NotImplementedException();
    }
}