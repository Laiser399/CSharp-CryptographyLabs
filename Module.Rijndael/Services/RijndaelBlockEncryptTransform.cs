using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelBlockEncryptTransform : IBlockCryptoTransform
{
    public int InputBlockSize => _rijndaelParameters.BlockSize;
    public int OutputBlockSize => _rijndaelParameters.BlockSize;

    private readonly IRijndaelParameters _rijndaelParameters;
    private readonly IRijndaelAddKeyService _rijndaelAddKeyService;
    private readonly IRijndaelSubstitutionService _rijndaelSubstitutionService;
    private readonly IRijndaelShiftRowsService _rijndaelShiftRowsService;
    private readonly IRijndaelMixColumnsService _rijndaelMixColumnsService;

    public RijndaelBlockEncryptTransform(
        IRijndaelParameters rijndaelParameters,
        IRijndaelAddKeyService rijndaelAddKeyService,
        IRijndaelSubstitutionService rijndaelSubstitutionService,
        IRijndaelShiftRowsService rijndaelShiftRowsService,
        IRijndaelMixColumnsService rijndaelMixColumnsService)
    {
        _rijndaelParameters = rijndaelParameters;
        _rijndaelAddKeyService = rijndaelAddKeyService;
        _rijndaelSubstitutionService = rijndaelSubstitutionService;
        _rijndaelShiftRowsService = rijndaelShiftRowsService;
        _rijndaelMixColumnsService = rijndaelMixColumnsService;
    }

    public void Transform(Span<byte> input, Span<byte> output)
    {
        ValidateArguments(input, output);

        input.CopyTo(output);

        _rijndaelAddKeyService.AddKey(output, _rijndaelParameters.InitialKey);

        for (var i = 0; i < _rijndaelParameters.RoundCount; i++)
        {
            _rijndaelSubstitutionService.SubstituteBytes(output);
            _rijndaelShiftRowsService.ShiftRows(output);

            if (i < _rijndaelParameters.RoundCount - 1)
            {
                _rijndaelMixColumnsService.MixColumns(output);
            }

            _rijndaelAddKeyService.AddKey(output, _rijndaelParameters.GetRoundKey(i));
        }
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
}