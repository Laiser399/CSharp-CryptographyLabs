using Autofac;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Rijndael.Cryptography;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelBlockCryptoTransformFactory : IBlockCryptoTransformFactory<IRijndaelParameters>
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IRijndaelKeyFactory _rijndaelKeyFactory;
    private readonly IRijndaelBlockCryptoTransformParametersFactory _rijndaelBlockCryptoTransformParametersFactory;

    public RijndaelBlockCryptoTransformFactory(
        ILifetimeScope lifetimeScope,
        IRijndaelKeyFactory rijndaelKeyFactory,
        IRijndaelBlockCryptoTransformParametersFactory rijndaelBlockCryptoTransformParametersFactory)
    {
        _lifetimeScope = lifetimeScope;
        _rijndaelKeyFactory = rijndaelKeyFactory;
        _rijndaelBlockCryptoTransformParametersFactory = rijndaelBlockCryptoTransformParametersFactory;
    }

    public IBlockCryptoTransform Create(
        TransformDirection direction,
        IRijndaelParameters parameters)
    {
        var key = _rijndaelKeyFactory.Create(parameters.Key);
        var blockCryptoTransformParameters = _rijndaelBlockCryptoTransformParametersFactory.Create(
            key,
            parameters.BlockSize
        );

        return direction switch
        {
            TransformDirection.Encrypt => _lifetimeScope.Resolve<RijndaelBlockEncryptTransform>(
                new TypedParameter(typeof(IRijndaelBlockCryptoTransformParameters), blockCryptoTransformParameters)
            ),
            TransformDirection.Decrypt => _lifetimeScope.Resolve<RijndaelBlockDecryptTransform>(
                new TypedParameter(typeof(IRijndaelBlockCryptoTransformParameters), blockCryptoTransformParameters)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported transform direction.")
        };
    }
}