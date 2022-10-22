using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelBlockCryptoTransformParametersFactory : IRijndaelBlockCryptoTransformParametersFactory
{
    private readonly IRijndaelExtendedKeyGenerator _rijndaelExtendedKeyGenerator;
    private readonly IRijndaelRoundCountCalculator _rijndaelRoundCountCalculator;

    public RijndaelBlockCryptoTransformParametersFactory(
        IRijndaelExtendedKeyGenerator rijndaelExtendedKeyGenerator,
        IRijndaelRoundCountCalculator rijndaelRoundCountCalculator)
    {
        _rijndaelExtendedKeyGenerator = rijndaelExtendedKeyGenerator;
        _rijndaelRoundCountCalculator = rijndaelRoundCountCalculator;
    }

    public IRijndaelBlockCryptoTransformParameters Create(IRijndaelKey key, RijndaelSize blockSize)
    {
        var extendedKey = _rijndaelExtendedKeyGenerator.Generate(key, blockSize);

        return new RijndaelBlockCryptoTransformParameters(
            extendedKey,
            blockSize.ByteCount,
            _rijndaelRoundCountCalculator.GetRoundCount(blockSize, key.Size)
        );
    }

    private class RijndaelBlockCryptoTransformParameters : IRijndaelBlockCryptoTransformParameters
    {
        public int BlockSize { get; }
        public int RoundCount { get; }

        public ReadOnlySpan<byte> InitialKey => new(_extendedKey, 0, BlockSize);

        private readonly byte[] _extendedKey;

        public RijndaelBlockCryptoTransformParameters(byte[] extendedKey, int blockSize, int roundCount)
        {
            _extendedKey = extendedKey;
            BlockSize = blockSize;
            RoundCount = roundCount;
        }

        public ReadOnlySpan<byte> GetRoundKey(int round)
        {
            return new ReadOnlySpan<byte>(_extendedKey, (round + 1) * BlockSize, BlockSize);
        }
    }
}