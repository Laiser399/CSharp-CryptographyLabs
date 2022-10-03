﻿using System.Numerics;
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

    // Alternatives: https://stackoverflow.com/questions/3432412/calculate-square-root-of-a-biginteger-system-numerics-biginteger
    public BigInteger FourthRoot(BigInteger value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Value could not be negative.", nameof(value));
        }

        if (value <= ulong.MaxValue)
        {
            return (BigInteger)Math.Pow((double)value, 0.25);
        }

        var left = (BigInteger)Math.Pow(ulong.MaxValue, 0.25);
        var right = value / (left * left * left);

        while (true)
        {
            var middle = (left + right) >> 1;

            var middle4 = BigInteger.Pow(middle, 4);
            if (value >= middle4
                && value <= BigInteger.Pow(middle + 1, 4))
            {
                return middle;
            }

            if (middle4 < value)
            {
                left = middle;
            }
            else
            {
                right = middle;
            }
        }
    }
}