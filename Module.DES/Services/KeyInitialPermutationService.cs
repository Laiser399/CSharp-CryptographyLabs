using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class KeyInitialPermutationService : IUInt64BitPermutationService
{
    private static readonly IReadOnlyList<ulong> Masks = new ulong[]
    {
        0x5500550055005500,
        0x3333000033330000,
        0xF0F0F0F00000000,
        0x3000C00000000,
        0x0,
        0xF1F2F4F8,
        0x6C9C0000CCCC,
        0xA500A500AA00AA,
        0xF0F0F0F0F0F0F0F,
        0x3303333000003333,
        0x5505500550550055,
    };

    private readonly IBitPermutationService _bitPermutationService;

    public KeyInitialPermutationService(IBitPermutationService bitPermutationService)
    {
        _bitPermutationService = bitPermutationService;
    }

    public ulong Permute(ulong value)
    {
        return _bitPermutationService.Permute(value, Masks) & 0x00_ff_ff_ff_ff_ff_ff_ff;
    }
}