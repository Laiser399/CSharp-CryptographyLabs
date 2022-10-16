using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelBlockTransformService : IRijndaelBlockTransformService
{
    private readonly IRijndaelParameters _rijndaelParameters;

    public RijndaelBlockTransformService(IRijndaelParameters rijndaelParameters)
    {
        _rijndaelParameters = rijndaelParameters;
    }

    public void Encrypt(Span<byte> input, Span<byte> output)
    {
        ValidateArguments(input, output);

        input.CopyTo(output);

        AddKey(output, _rijndaelParameters.InitialKey);

        for (var i = 0; i < _rijndaelParameters.RoundCount; i++)
        {
            SubBytes(output);
            ShiftRows(output);

            if (i < _rijndaelParameters.RoundCount - 1)
            {
                MixColumns(output);
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
                InverseMixColumns(output);
            }

            InverseShiftRows(output);
            InverseSubBytes(output);
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

    private void SubBytes(Span<byte> state)
    {
        throw new NotImplementedException();
    }

    private void InverseSubBytes(Span<byte> state)
    {
        throw new NotImplementedException();
    }

    private void ShiftRows(Span<byte> state)
    {
        throw new NotImplementedException();
    }

    private void InverseShiftRows(Span<byte> state)
    {
        throw new NotImplementedException();
    }

    private void MixColumns(Span<byte> state)
    {
        throw new NotImplementedException();
    }

    private void InverseMixColumns(Span<byte> state)
    {
        throw new NotImplementedException();
    }
}