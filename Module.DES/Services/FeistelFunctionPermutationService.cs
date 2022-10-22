using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class FeistelFunctionPermutationService : IUInt32BitPermutationService
{
    private static readonly IReadOnlyList<uint> Masks = new uint[]
    {
        0x44015150,
        0x13022300,
        0x7010200,
        0x50000,
        0x995E,
        0x380071,
        0x90D040C,
        0x3010013,
        0x54055004,
    };

    private readonly IBitPermutationService _bitPermutationService;

    public FeistelFunctionPermutationService(IBitPermutationService bitPermutationService)
    {
        _bitPermutationService = bitPermutationService;
    }

    public uint Permute(uint value)
    {
        return _bitPermutationService.Permute(value, Masks);
    }
}