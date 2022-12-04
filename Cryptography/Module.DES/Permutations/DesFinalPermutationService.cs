using Module.DES.Permutations.Abstract;
using Module.PermutationNetwork.Services.Abstract;

namespace Module.DES.Permutations;

public class DesFinalPermutationService : IUInt64BitPermutationService
{
    private static readonly IReadOnlyList<ulong> Masks = new ulong[]
    {
        0x5555555500000000,
        0x3300330033003300,
        0xF0F00000F0F0000,
        0xFF00FF00000000,
        0xFFFF00000000,
        0xF0F00F0F,
        0x3CC30000C33C,
        0x99006600990066,
        0xF0F0F0F,
        0x333300003333,
        0x5500550055005500,
    };

    private readonly IBitPermutationService _bitPermutationService;

    public DesFinalPermutationService(IBitPermutationService bitPermutationService)
    {
        _bitPermutationService = bitPermutationService;
    }

    public ulong Permute(ulong value)
    {
        return _bitPermutationService.Permute(value, Masks);
    }
}