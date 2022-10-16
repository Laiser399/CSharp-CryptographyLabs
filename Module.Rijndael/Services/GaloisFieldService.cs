using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class GaloisFieldService : IGaloisFieldService
{
    public bool IsGeneratingElement(ushort value)
    {
        return value is >= 0b1_0000_0000 and <= 0b1_1111_1111
               && IsIrreducible(value);
    }

    private static bool IsIrreducible(ushort value)
    {
        if ((value & 1) == 0)
        {
            return false;
        }

        for (ushort i = 0b11; i < value; i += 2)
        {
            if (Mod(value, i) == 0)
            {
                return false;
            }
        }

        return true;
    }

    private static ushort Mod(ushort dividend, ushort divisor)
    {
        var divisorBitLength = GetBitLength(divisor);

        while (true)
        {
            var dividendBitLength = GetBitLength(dividend);
            if (dividendBitLength < divisorBitLength)
            {
                break;
            }

            dividend ^= (ushort)(divisor << (dividendBitLength - divisorBitLength));
        }

        return dividend;
    }

    private static int GetBitLength(ushort value)
    {
        var result = 0;
        while (value > 0)
        {
            result++;
            value >>= 1;
        }

        return result;
    }
}