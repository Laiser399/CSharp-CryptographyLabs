using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

// https://stackoverflow.com/a/48855115/12158229
public class RandomBigIntegerGenerator : IRandomBigIntegerGenerator
{
    public BigInteger Generate(BigInteger min, BigInteger max, Random random)
    {
        if (min >= max)
        {
            throw new ArgumentException("Min is greater or equal max value.");
        }

        return Generate(max - min, random) + min;
    }

    public BigInteger Generate(BigInteger max, Random random)
    {
        if (max <= 0)
        {
            throw new ArgumentException("Max value must be positive.", nameof(max));
        }

        var bytes = max.ToByteArray();

        var zeroBitsMask = GetZeroBitsMask(bytes[^1]);

        // В худшем случае вероятность получить значение больше или равное max равна 0.5
        while (true)
        {
            random.NextBytes(bytes);
            bytes[^1] &= zeroBitsMask;
            var result = new BigInteger(bytes);
            if (result < max)
            {
                return result;
            }
        }
    }

    private static byte GetZeroBitsMask(byte mostSignificantByte)
    {
        for (var i = 7; i >= 0; i--)
        {
            if ((mostSignificantByte & (0b1 << i)) != 0)
            {
                var zeroBitsCount = 7 - i;
                return (byte)(0b11111111 >> zeroBitsCount);
            }
        }

        return 0b00000000;
    }
}