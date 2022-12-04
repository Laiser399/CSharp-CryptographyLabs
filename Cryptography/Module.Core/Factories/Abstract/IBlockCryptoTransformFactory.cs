using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;

namespace Module.Core.Factories.Abstract;

public interface IBlockCryptoTransformFactory<in T>
{
    /// <exception cref="ArgumentException"></exception>
    IBlockCryptoTransform Create(TransformDirection direction, T parameters);
}