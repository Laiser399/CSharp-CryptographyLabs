using System.Security.Cryptography;
using Module.Core.Enums;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Factories.Abstract;

public interface IRijndaelCryptoTransformFactory
{
    /// <exception cref="ArgumentException"></exception>
    ICryptoTransform CreateECB(TransformDirection direction, byte[] key, RijndaelSize blockSize);

    /// <exception cref="ArgumentException"></exception>
    ICryptoTransform Create(
        BlockCouplingMode mode,
        TransformDirection direction,
        byte[] initialVector,
        byte[] key,
        RijndaelSize blockSize);
}