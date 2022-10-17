using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class XorService : IXorService
{
    public void Xor(ReadOnlySpan<byte> first, ReadOnlySpan<byte> second, Span<byte> result)
    {
        if (first.Length != second.Length || first.Length != result.Length)
        {
            throw new ArgumentException("Lengths of arguments are not equal.");
        }

        var blockCount = first.Length / sizeof(ulong);

        unsafe
        {
            fixed (byte* firstPtr = first, secondPtr = second, resultPtr = result)
            {
                var firstULongPtr = (ulong*)firstPtr;
                var secondULongPtr = (ulong*)secondPtr;
                var resultULongPtr = (ulong*)resultPtr;

                for (var i = 0; i < blockCount; i++)
                {
                    resultULongPtr[i] = firstULongPtr[i] ^ secondULongPtr[i];
                }
            }
        }

        for (var i = blockCount * sizeof(ulong); i < first.Length; i++)
        {
            result[i] = (byte)(first[i] ^ second[i]);
        }
    }
}