using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelAddKeyService : IRijndaelAddKeyService
{
    public unsafe void AddKey(Span<byte> state, ReadOnlySpan<byte> key)
    {
        ValidateArguments(state, key);

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

    private static void ValidateArguments(Span<byte> state, ReadOnlySpan<byte> key)
    {
        if (state.Length == 0)
        {
            throw new ArgumentException("State is empty.", nameof(state));
        }

        if (key.Length == 0)
        {
            throw new ArgumentException("Key is empty.", nameof(key));
        }

        if (state.Length != key.Length)
        {
            throw new ArgumentException("State and key sizes are not equal.");
        }
    }
}