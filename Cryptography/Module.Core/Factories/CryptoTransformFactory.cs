using System.Security.Cryptography;
using Autofac;
using Module.Core.Cryptography;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Core.Services.Abstract;

namespace Module.Core.Factories;

public class CryptoTransformFactory<T> : ICryptoTransformFactory<T>
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IBlockCryptoTransformFactory<T> _blockCryptoTransformFactory;

    public CryptoTransformFactory(
        ILifetimeScope lifetimeScope,
        IBlockCryptoTransformFactory<T> blockCryptoTransformFactory)
    {
        _lifetimeScope = lifetimeScope;
        _blockCryptoTransformFactory = blockCryptoTransformFactory;
    }

    public ICryptoTransform CreateEcb(
        TransformDirection direction,
        T parameters,
        bool withParallelism)
    {
        var blockCryptoTransform = _blockCryptoTransformFactory.Create(direction, parameters);

        return (direction, withParallelism) switch
        {
            (TransformDirection.Encrypt, true) => new EcbEncryptParallelTransform(blockCryptoTransform),
            (TransformDirection.Encrypt, false) => new EcbEncryptTransform(blockCryptoTransform),
            (TransformDirection.Decrypt, true) => new EcbDecryptParallelTransform(blockCryptoTransform),
            (TransformDirection.Decrypt, false) => new EcbDecryptTransform(blockCryptoTransform),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported transform direction.")
        };
    }

    public ICryptoTransform Create(
        TransformDirection direction,
        T parameters,
        BlockCipherMode mode,
        byte[] initialVector)
    {
        return (mode, direction) switch
        {
            (BlockCipherMode.CBC, TransformDirection.Encrypt) => new CbcEncryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Encrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            (BlockCipherMode.CBC, TransformDirection.Decrypt) => new CbcDecryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Decrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            (BlockCipherMode.CFB, TransformDirection.Encrypt) => new CfbEncryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Encrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            (BlockCipherMode.CFB, TransformDirection.Decrypt) => new CfbDecryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Encrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            (BlockCipherMode.OFB, TransformDirection.Encrypt) => new OfbEncryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Encrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            (BlockCipherMode.OFB, TransformDirection.Decrypt) => new OfbDecryptTransform(
                _blockCryptoTransformFactory.Create(TransformDirection.Encrypt, parameters),
                initialVector,
                _lifetimeScope.Resolve<IXorService>()
            ),
            _ => throw new ArgumentOutOfRangeException("", "Unsupported mode or transform direction.")
        };
    }
}