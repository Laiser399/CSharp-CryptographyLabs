using Module.Core.Cryptography.Abstract;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelBlockDecryptTransform : IBlockCryptoTransform
{
    public int InputBlockSize => _rijndaelParameters.BlockSize;
    public int OutputBlockSize => _rijndaelParameters.BlockSize;

    private readonly IRijndaelParameters _rijndaelParameters;
    private readonly IXorService _xorService;
    private readonly IRijndaelSubstitutionService _rijndaelSubstitutionService;
    private readonly IRijndaelShiftRowsService _rijndaelShiftRowsService;
    private readonly IRijndaelMixColumnsService _rijndaelMixColumnsService;

    public RijndaelBlockDecryptTransform(
        IRijndaelParameters rijndaelParameters,
        IXorService xorService,
        IRijndaelSubstitutionService rijndaelSubstitutionService,
        IRijndaelShiftRowsService rijndaelShiftRowsService,
        IRijndaelMixColumnsService rijndaelMixColumnsService)
    {
        _rijndaelParameters = rijndaelParameters;
        _xorService = xorService;
        _rijndaelSubstitutionService = rijndaelSubstitutionService;
        _rijndaelShiftRowsService = rijndaelShiftRowsService;
        _rijndaelMixColumnsService = rijndaelMixColumnsService;
    }

    public void Transform(Span<byte> input, Span<byte> output)
    {
        ValidateArguments(input, output);

        input.CopyTo(output);

        for (var i = _rijndaelParameters.RoundCount - 1; i >= 0; i--)
        {
            AddKey(output, _rijndaelParameters.GetRoundKey(i));

            if (i < _rijndaelParameters.RoundCount - 1)
            {
                _rijndaelMixColumnsService.ReverseColumnsMixing(output);
            }

            _rijndaelShiftRowsService.InverseShiftRows(output);
            _rijndaelSubstitutionService.SubstituteBytesInversed(output);
        }

        AddKey(output, _rijndaelParameters.InitialKey);
    }

    private void ValidateArguments(Span<byte> input, Span<byte> output)
    {
        if (input.Length != _rijndaelParameters.BlockSize)
        {
            throw new ArgumentException("Invalid length of input span size.", nameof(input));
        }

        if (output.Length != _rijndaelParameters.BlockSize)
        {
            throw new ArgumentException("Invalid length of output span size.", nameof(output));
        }
    }

    private void AddKey(Span<byte> state, ReadOnlySpan<byte> key)
    {
        _xorService.Xor(state, key, state);
    }
}