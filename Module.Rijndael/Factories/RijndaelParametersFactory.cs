using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelParametersFactory : IRijndaelParametersFactory
{
    private readonly IRijndaelExtendedKeyGenerator _rijndaelExtendedKeyGenerator;
    private readonly IRijndaelRoundCountCalculator _rijndaelRoundCountCalculator;

    public RijndaelParametersFactory(
        IRijndaelExtendedKeyGenerator rijndaelExtendedKeyGenerator,
        IRijndaelRoundCountCalculator rijndaelRoundCountCalculator)
    {
        _rijndaelExtendedKeyGenerator = rijndaelExtendedKeyGenerator;
        _rijndaelRoundCountCalculator = rijndaelRoundCountCalculator;
    }

    public IRijndaelParameters Create(IRijndaelKey key, RijndaelSize blockSize)
    {
        var extendedKey = _rijndaelExtendedKeyGenerator.Generate(key, blockSize);

        return new RijndaelParameters(
            key,
            extendedKey,
            blockSize.ByteCount,
            _rijndaelRoundCountCalculator.GetRoundCount(blockSize, key.Size)
        );
    }

    private class RijndaelParameters : IRijndaelParameters
    {
        public int BlockSize { get; }
        public int RoundCount { get; }

        public ReadOnlySpan<byte> InitialKey => _key.Key;

        private readonly IRijndaelKey _key;
        private readonly byte[] _extendedKey;

        public RijndaelParameters(IRijndaelKey key, byte[] extendedKey, int blockSize, int roundCount)
        {
            _key = key;
            _extendedKey = extendedKey;
            BlockSize = blockSize;
            RoundCount = roundCount;
        }

        public ReadOnlySpan<byte> GetRoundKey(int round)
        {
            return new ReadOnlySpan<byte>(_extendedKey, BlockSize * round, BlockSize);
        }
    }
}