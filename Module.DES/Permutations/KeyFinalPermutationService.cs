using Module.DES.Permutations.Abstract;
using Module.PermutationNetwork.Services.Abstract;

namespace Module.DES.Permutations;

public class KeyFinalPermutationService : IUInt64BitPermutationService
{
    private static readonly IReadOnlyList<ulong> Masks = new ulong[]
    {
        0x40105400404450,
        0x120001311000,
        0x30C040A060600,
        0xD9001800950000,
        0x3400000000,
        0x88340100,
        0x5910000015C2,
        0xD900720034001A,
        0xA0802060F0B05,
        0x20100122210201,
        0x14115040011105,
    };

    private readonly IBitPermutationService _bitPermutationService;

    public KeyFinalPermutationService(IBitPermutationService bitPermutationService)
    {
        _bitPermutationService = bitPermutationService;
    }

    public ulong Permute(ulong value)
    {
        return _bitPermutationService.Permute(value, Masks) & 0x0000ffff_ffffffff;
    }
}