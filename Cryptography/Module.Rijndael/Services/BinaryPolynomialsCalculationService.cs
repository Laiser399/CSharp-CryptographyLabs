using Module.Core.Extensions;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class BinaryPolynomialsCalculationService : IBinaryPolynomialsCalculationService
{
    public ulong Multiply(uint a, uint b)
    {
        var bExt = (ulong)b;
        var result = 0ul;
        while (a > 0)
        {
            if ((a & 1) == 1)
            {
                result ^= bExt;
            }

            a >>= 1;
            bExt <<= 1;
        }

        return result;
    }

    public uint Divide(uint a, uint b, out uint residual)
    {
        var result = Divide(a, b, out ulong residualTemp);
        residual = (uint)residualTemp;
        return (uint)result;
    }

    private static ulong Divide(ulong a, ulong b, out ulong residual)
    {
        var aBitLength = a.GetBitLength();
        var bBitLength = b.GetBitLength();

        ulong result = 0;

        while (aBitLength >= bBitLength)
        {
            var diff = aBitLength - bBitLength;
            a ^= b << diff;
            result ^= 1ul << diff;
            aBitLength = a.GetBitLength();
        }

        residual = a;
        return result;
    }

    public uint GreatestCommonDivisor(uint a, uint b, out uint x, out uint y)
    {
        (x, y) = (1, 0);
        var (xCurrent, yCurrent) = (0u, 1u);

        while (b > 0)
        {
            var q = Divide(a, b, out var residual);
            (a, b) = (b, residual);
            (xCurrent, x) = ((uint)(x ^ Multiply(q, xCurrent)), xCurrent);
            (yCurrent, y) = ((uint)(y ^ Multiply(q, yCurrent)), yCurrent);
        }

        return a;
    }
}