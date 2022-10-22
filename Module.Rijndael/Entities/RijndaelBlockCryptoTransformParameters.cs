using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Entities;

public class RijndaelBlockCryptoTransformParameters : IRijndaelBlockCryptoTransformParameters
{
    public IReadOnlyList<byte> Key { get; }
    public RijndaelSize BlockSize { get; }

    public RijndaelBlockCryptoTransformParameters(IReadOnlyList<byte> key, RijndaelSize blockSize)
    {
        Key = key;
        BlockSize = blockSize;
    }
}