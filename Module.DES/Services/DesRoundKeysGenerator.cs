using Autofac.Features.Indexed;
using Module.DES.Enums;
using Module.DES.Permutations.Abstract;
using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class DesRoundKeysGenerator : IDesRoundKeysGenerator
{
    private static readonly IReadOnlyList<byte> RoundShifts = new byte[]
    {
        1, 1, 2, 2, 2, 2, 2, 2,
        1, 2, 2, 2, 2, 2, 2, 1
    };

    private readonly IBitOperationsService _bitOperationsService;
    private readonly IUInt64BitPermutationService _keyInitialPermutationService;
    private readonly IUInt64BitPermutationService _keyFinalPermutationService;

    public DesRoundKeysGenerator(
        IBitOperationsService bitOperationsService,
        IIndex<PermutationType, IUInt64BitPermutationService> permutationServices)
    {
        _bitOperationsService = bitOperationsService;
        _keyInitialPermutationService = permutationServices[PermutationType.KeyInitial];
        _keyFinalPermutationService = permutationServices[PermutationType.KeyFinal];
    }

    public ulong[] Generate(ulong key56)
    {
        var key64 = ExpandTo64(key56);

        var permuted = _keyInitialPermutationService.Permute(key64);

        var c = (uint)permuted & 0x0fffffff;
        var d = (uint)permuted >> 28;

        var roundKeys48 = new ulong[16];
        for (var i = 0; i < 16; i++)
        {
            var shift = RoundShifts[i];
            c = ((c << shift) & 0x0fffffff) | (c >> (28 - shift));
            d = ((d << shift) & 0x0fffffff) | (d >> (28 - shift));

            roundKeys48[i] = ((ulong)d << 28) | c;

            roundKeys48[i] = _keyFinalPermutationService.Permute(roundKeys48[i]);
        }

        return roundKeys48;
    }

    private ulong ExpandTo64(ulong key56)
    {
        var key64 = 0ul;
        for (var i = 0; i < 8; ++i)
        {
            var bitSequence = (byte)(key56 & 0b01111111);
            key56 >>= 7;

            var xorResult = _bitOperationsService.XorBits(bitSequence);

            key64 = (key64 << 1) | (byte)(~xorResult & 1);
            key64 = (key64 << 7) | bitSequence;
        }

        return key64;
    }
}