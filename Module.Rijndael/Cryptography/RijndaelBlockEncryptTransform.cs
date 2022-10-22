using Module.Core.Cryptography.Abstract;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Cryptography;

public class RijndaelBlockEncryptTransform : IBlockCryptoTransform
{
    public int InputBlockSize => _parameters.BlockSize;
    public int OutputBlockSize => _parameters.BlockSize;

    private readonly IRijndaelBlockCryptoTransformParameters _parameters;
    private readonly IXorService _xorService;
    private readonly IRijndaelSubstitutionService _rijndaelSubstitutionService;
    private readonly IRijndaelShiftRowsService _rijndaelShiftRowsService;
    private readonly IRijndaelMixColumnsService _rijndaelMixColumnsService;

    public RijndaelBlockEncryptTransform(
        IRijndaelBlockCryptoTransformParameters parameters,
        IXorService xorService,
        IRijndaelSubstitutionService rijndaelSubstitutionService,
        IRijndaelShiftRowsService rijndaelShiftRowsService,
        IRijndaelMixColumnsService rijndaelMixColumnsService)
    {
        _parameters = parameters;
        _xorService = xorService;
        _rijndaelSubstitutionService = rijndaelSubstitutionService;
        _rijndaelShiftRowsService = rijndaelShiftRowsService;
        _rijndaelMixColumnsService = rijndaelMixColumnsService;
    }

    public void Transform(ReadOnlySpan<byte> input, Span<byte> output)
    {
        ValidateArguments(input, output);

        input.CopyTo(output);

        AddKey(output, _parameters.InitialKey);

        for (var i = 0; i < _parameters.RoundCount; i++)
        {
            _rijndaelSubstitutionService.SubstituteBytes(output);
            _rijndaelShiftRowsService.ShiftRows(output);

            if (i < _parameters.RoundCount - 1)
            {
                _rijndaelMixColumnsService.MixColumns(output);
            }

            AddKey(output, _parameters.GetRoundKey(i));
        }
    }

    private void ValidateArguments(ReadOnlySpan<byte> input, Span<byte> output)
    {
        if (input.Length != _parameters.BlockSize)
        {
            throw new ArgumentException("Invalid length of input span size.", nameof(input));
        }

        if (output.Length != _parameters.BlockSize)
        {
            throw new ArgumentException("Invalid length of output span size.", nameof(output));
        }
    }

    private void AddKey(Span<byte> state, ReadOnlySpan<byte> key)
    {
        _xorService.Xor(state, key, state);
    }
}