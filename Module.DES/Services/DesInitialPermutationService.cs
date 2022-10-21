using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class DesInitialPermutationService : IUInt64BitPermutationService
{
    private static readonly IReadOnlyList<ulong> Masks = new ulong[]
    {
        0x5500550055005500,
        0x3333000033330000,
        0xF0F0F0F00000000,
        0xFF000000FF0000,
        0xFFFF00000000,
        0x55AAAA55,
        0xA55A00005AA5,
        0x66006600990099,
        0xF0F00000F0F,
        0x33003300330033,
        0x5555555500000000,
    };

    private readonly IBitPermutationService _bitPermutationService;

    public DesInitialPermutationService(IBitPermutationService bitPermutationService)
    {
        _bitPermutationService = bitPermutationService;
    }

    public ulong Permute(ulong value)
    {
        return _bitPermutationService.Permute(value, Masks);
    }
}