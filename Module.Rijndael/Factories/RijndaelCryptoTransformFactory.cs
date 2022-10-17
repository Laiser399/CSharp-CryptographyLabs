using System.Security.Cryptography;
using Autofac;
using Module.Core.Enums;
using Module.Core.Services;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelCryptoTransformFactory : IRijndaelCryptoTransformFactory
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IRijndaelKeyFactory _rijndaelKeyFactory;
    private readonly IRijndaelParametersFactory _rijndaelParametersFactory;

    public RijndaelCryptoTransformFactory(
        ILifetimeScope lifetimeScope,
        IRijndaelKeyFactory rijndaelKeyFactory,
        IRijndaelParametersFactory rijndaelParametersFactory)
    {
        _lifetimeScope = lifetimeScope;
        _rijndaelKeyFactory = rijndaelKeyFactory;
        _rijndaelParametersFactory = rijndaelParametersFactory;
    }

    public ICryptoTransform CreateECB(TransformDirection direction, byte[] key, RijndaelSize blockSize)
    {
        var blockCryptoTransform = GetBlockCryptoTransform(direction, key, blockSize);

        return direction switch
        {
            TransformDirection.Encrypt => new EcbEncryptTransform(blockCryptoTransform),
            TransformDirection.Decrypt => new EcbDecryptTransform(blockCryptoTransform),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported transform direction.")
        };
    }

    public ICryptoTransform Create(
        BlockCipherMode mode,
        TransformDirection direction,
        byte[] initialVector,
        byte[] key,
        RijndaelSize blockSize)
    {
        return (mode, direction) switch
        {
            (BlockCipherMode.CBC, _) => new CbcEncryptTransform(
                GetBlockCryptoTransform(direction, key, blockSize),
                initialVector
            ),
            (BlockCipherMode.CFB, TransformDirection.Encrypt) => new CfbEncryptTransform(
                GetBlockCryptoTransform(TransformDirection.Encrypt, key, blockSize),
                initialVector
            ),
            (BlockCipherMode.CFB, TransformDirection.Decrypt) => new CfbDecryptTransform(
                GetBlockCryptoTransform(TransformDirection.Encrypt, key, blockSize),
                initialVector
            ),
            (BlockCipherMode.OFB, TransformDirection.Encrypt) => new OfbEncryptTransform(
                GetBlockCryptoTransform(TransformDirection.Encrypt, key, blockSize),
                initialVector
            ),
            (BlockCipherMode.OFB, TransformDirection.Decrypt) => new OfbDecryptTransform(
                GetBlockCryptoTransform(TransformDirection.Encrypt, key, blockSize),
                initialVector
            ),
            _ => throw new ArgumentOutOfRangeException("", "Unsupported mode or transform direction.")
        };
    }

    private IBlockCryptoTransform GetBlockCryptoTransform(
        TransformDirection direction,
        byte[] key,
        RijndaelSize blockSize)
    {
        var rijndaelKey = _rijndaelKeyFactory.Create(key);
        var rijndaelParameters = _rijndaelParametersFactory.Create(rijndaelKey, blockSize);

        return _lifetimeScope.ResolveKeyed<IBlockCryptoTransform>(
            direction,
            new TypedParameter(typeof(IRijndaelParameters), rijndaelParameters)
        );
    }
}