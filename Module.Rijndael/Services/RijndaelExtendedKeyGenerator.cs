using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelExtendedKeyGenerator : IRijndaelExtendedKeyGenerator
{
    private static readonly uint[] RoundConstants =
    {
        0x1u, 0x2u, 0x4u, 0x8u,
        0x10u, 0x20u, 0x40u, 0x80u,
        0x1Bu, 0x36u, 0x6Cu, 0xD8u,
        0xABu, 0x4Du, 0x9Au, 0x2Fu,
        0x5Eu, 0xBCu, 0x63u, 0xC6u,
        0x97u, 0x35u, 0x6Au, 0xD4u,
        0xB3u, 0x7Du, 0xFAu, 0xEFu,
        0xC5u
    };

    private readonly IRijndaelRoundCountCalculator _rijndaelRoundCountCalculator;
    private readonly IRijndaelSubstitutionService _rijndaelSubstitutionService;

    public RijndaelExtendedKeyGenerator(
        IRijndaelRoundCountCalculator rijndaelRoundCountCalculator,
        IRijndaelSubstitutionService rijndaelSubstitutionService)
    {
        _rijndaelRoundCountCalculator = rijndaelRoundCountCalculator;
        _rijndaelSubstitutionService = rijndaelSubstitutionService;
    }

    public unsafe byte[] Generate(IRijndaelKey key, RijndaelSize blockSize)
    {
        var roundCount = _rijndaelRoundCountCalculator.GetRoundCount(blockSize, key.Size);

        var extendedKeyByteCount = roundCount * blockSize.ByteCount;
        var extendedKeyWordCount = roundCount * blockSize.WordCount;
        var extendedKey = new byte[extendedKeyByteCount];

        fixed (byte* extendedKeyPtr = extendedKey, keyPtr = key.Key)
        {
            var extendedKeyUIntPtr = (uint*)extendedKeyPtr;
            var keyUIntPtr = (uint*)keyPtr;

            for (var i = 0; i < extendedKeyWordCount; i++)
            {
                var prevWordPtr = i == 0
                    ? keyUIntPtr + key.Size.WordCount - 1
                    : extendedKeyUIntPtr + i - 1;

                var prevBlockWord = i < key.Size.WordCount
                    ? keyUIntPtr[i]
                    : extendedKeyUIntPtr[i - key.Size.WordCount];

                if (i % key.Size.WordCount == 0)
                {
                    extendedKeyUIntPtr[i] = RotateWord(*prevWordPtr);
                    _rijndaelSubstitutionService.SubstituteBytes(new Span<byte>(extendedKeyPtr + i * 4, 4));

                    extendedKeyUIntPtr[i] ^= prevBlockWord;
                    extendedKeyUIntPtr[i] ^= RoundConstants[i / key.Size.WordCount];
                }
                else if (key.Size == RijndaelSize.S256 && i % key.Size.WordCount == 4)
                {
                    extendedKeyUIntPtr[i] = *prevWordPtr;
                    _rijndaelSubstitutionService.SubstituteBytes(new Span<byte>(extendedKeyPtr + i * 4, 4));
                    extendedKeyUIntPtr[i] ^= prevBlockWord;
                }
                else
                {
                    extendedKeyUIntPtr[i] = *prevWordPtr ^ prevBlockWord;
                }
            }
        }

        return extendedKey;
    }

    private static uint RotateWord(uint word)
    {
        return (word >> 8) | (word << 24);
    }
}