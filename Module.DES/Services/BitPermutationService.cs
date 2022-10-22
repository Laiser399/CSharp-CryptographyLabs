using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class BitPermutationService : IBitPermutationService
{
    private static readonly byte[] Deltas32 = { 1, 2, 4, 8, 16, 8, 4, 2, 1 };
    private static readonly byte[] Deltas64 = { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };

    public uint Permute(uint value, IReadOnlyList<uint> masks)
    {
        if (masks.Count != 9)
        {
            throw new ArgumentException("Wrong masks count.");
        }

        for (var i = 0; i < Deltas32.Length; ++i)
        {
            if (masks[i] != 0)
            {
                value = SwapBits(value, Deltas32[i], masks[i]);
            }
        }

        return value;
    }

    public ulong Permute(ulong value, IReadOnlyList<ulong> masks)
    {
        if (masks.Count != 11)
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

    private static uint SwapBits(uint x, int delta, uint mask)
    {
        var y = (x ^ (x >> delta)) & mask;
        return x ^ y ^ (y << delta);
    }

    private static ulong SwapBits(ulong x, int delta, ulong mask)
    {
        var y = (x ^ (x >> delta)) & mask;
        return x ^ y ^ (y << delta);
    }
}