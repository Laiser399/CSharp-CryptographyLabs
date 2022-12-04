using Module.Core.Extensions;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class GaloisFieldService : IGaloisFieldService
{
    public IReadOnlyCollection<ushort> CalculateGeneratingElements()
    {
        var result = new List<ushort>();

        for (ushort x = 0b1_00000001; x <= 0b1_11111111; x += 2)
        {
            if (IsGeneratingElement(x))
            {
                result.Add(x);
            }
        }

        return result;
    }

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
        var divisorBitLength = divisor.GetBitLength();

        while (true)
        {
            var dividendBitLength = dividend.GetBitLength();
            if (dividendBitLength < divisorBitLength)
            {
                break;
            }

            dividend ^= (ushort)(divisor << (dividendBitLength - divisorBitLength));
        }

        return dividend;
    }
}