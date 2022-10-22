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
}