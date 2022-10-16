using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelBlockTransformService : IRijndaelBlockTransformService
{
    private readonly IRijndaelParameters _rijndaelParameters;
    private readonly IRijndaelSubstitutionService _rijndaelSubstitutionService;
    private readonly IRijndaelShiftRowsService _rijndaelShiftRowsService;
    private readonly IRijndaelMixColumnsService _rijndaelMixColumnsService;

    public RijndaelBlockTransformService(
        IRijndaelParameters rijndaelParameters,
        IRijndaelSubstitutionService rijndaelSubstitutionService,
        IRijndaelShiftRowsService rijndaelShiftRowsService,
        IRijndaelMixColumnsService rijndaelMixColumnsService)
    {
        _rijndaelParameters = rijndaelParameters;
        _rijndaelSubstitutionService = rijndaelSubstitutionService;
        _rijndaelShiftRowsService = rijndaelShiftRowsService;
        _rijndaelMixColumnsService = rijndaelMixColumnsService;
    }

    public void Encrypt(Span<byte> input, Span<byte> output)
    {
        ValidateArguments(input, output);

        input.CopyTo(output);

        AddKey(output, _rijndaelParameters.InitialKey);

        for (var i = 0; i < _rijndaelParameters.RoundCount; i++)
        {
            _rijndaelSubstitutionService.SubstituteBytes(output);
            _rijndaelShiftRowsService.ShiftRows(output);

            if (i < _rijndaelParameters.RoundCount - 1)
            {
                _rijndaelMixColumnsService.MixColumns(output);
            }

            AddKey(output, _rijndaelParameters.GetRoundKey(i));
        }
    }

    public void Decrypt(Span<byte> input, Span<byte> output)
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

    private static void AddKey(Span<byte> state, Span<byte> key)
    {
        unsafe
        {
            fixed (byte* statePtr = state, keyPtr = key)
            {
                var stateULongPtr = (ulong*)statePtr;
                var keyULongPtr = (ulong*)keyPtr;
                for (var i = 0; i < state.Length / sizeof(ulong); i++)
                {
                    stateULongPtr[i] ^= keyULongPtr[i];
                }
            }
        }
    }
}