using Module.Rijndael.Enums;

namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelBlockCryptoTransformParameters
{
    IReadOnlyList<byte> Key { get; }
    RijndaelSize BlockSize { get; }
}