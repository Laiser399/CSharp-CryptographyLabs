using Autofac;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Rijndael.Cryptography;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelBlockCryptoTransformFactory : IBlockCryptoTransformFactory<IRijndaelBlockCryptoTransformParameters>
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IRijndaelKeyFactory _rijndaelKeyFactory;
    private readonly IRijndaelParametersFactory _rijndaelParametersFactory;

    public RijndaelBlockCryptoTransformFactory(
        ILifetimeScope lifetimeScope,
        IRijndaelKeyFactory rijndaelKeyFactory,
        IRijndaelParametersFactory rijndaelParametersFactory)
    {
        _lifetimeScope = lifetimeScope;
        _rijndaelKeyFactory = rijndaelKeyFactory;
        _rijndaelParametersFactory = rijndaelParametersFactory;
    }

    public IBlockCryptoTransform Create(
        TransformDirection direction,
        IRijndaelBlockCryptoTransformParameters parameters)
    {
        var rijndaelKey = _rijndaelKeyFactory.Create(parameters.Key);
        var rijndaelParameters = _rijndaelParametersFactory.Create(rijndaelKey, parameters.BlockSize);

        return direction switch
        {
            TransformDirection.Encrypt => _lifetimeScope.Resolve<RijndaelBlockEncryptTransform>(
                new TypedParameter(typeof(IRijndaelParameters), rijndaelParameters)
            ),
            TransformDirection.Decrypt => _lifetimeScope.Resolve<RijndaelBlockDecryptTransform>(
                new TypedParameter(typeof(IRijndaelParameters), rijndaelParameters)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported transform direction.")
        };
    }
}