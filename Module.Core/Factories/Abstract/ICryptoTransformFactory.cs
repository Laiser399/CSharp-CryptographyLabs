using System.Security.Cryptography;
using Module.Core.Enums;

namespace Module.Core.Factories.Abstract;

public interface ICryptoTransformFactory<in T>
{
    /// <exception cref="ArgumentException"></exception>
    ICryptoTransform CreateEcb(
        TransformDirection direction,
        T parameters,
        bool withParallelism);

    /// <exception cref="ArgumentException"></exception>
    ICryptoTransform Create(
        TransformDirection direction,
        T parameters,
        BlockCipherMode mode,
        byte[] initialVector);
}