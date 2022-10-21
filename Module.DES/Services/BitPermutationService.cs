using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class BitPermutationService : IBitPermutationService
{
    private static readonly byte[] Deltas64 = { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };

    public ulong Permute(ulong value, ulong[] masks)
    {
        if (masks.Length != 11)
        {
            throw new ArgumentException("Wrong masks count.");
        }

        for (var i = 0; i < Deltas64.Length; ++i)
        {
            if (masks[i] != 0)
            {
                value = SwapBits(value, Deltas64[i], masks[i]);
            }
        }

        return value;
    }

    private static ulong SwapBits(ulong x, int delta, ulong mask)
    {
        var y = (x ^ (x >> delta)) & mask;
        return x ^ y ^ (y << delta);
    }
}