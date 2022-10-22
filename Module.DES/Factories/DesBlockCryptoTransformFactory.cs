using Autofac;
using Module.Core.Cryptography.Abstract;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.DES.Cryptography;
using Module.DES.Entities.Abstract;
using Module.DES.Factories.Abstract;

namespace Module.DES.Factories;

public class DesBlockCryptoTransformFactory : IBlockCryptoTransformFactory<IDesParameters>
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IDesKeyFactory _desKeyFactory;

    public DesBlockCryptoTransformFactory(
        ILifetimeScope lifetimeScope,
        IDesKeyFactory desKeyFactory)
    {
        _lifetimeScope = lifetimeScope;
        _desKeyFactory = desKeyFactory;
    }

    public IBlockCryptoTransform Create(TransformDirection direction, IDesParameters parameters)
    {
        var key = _desKeyFactory.Create(parameters.Key56);

        return direction switch
        {
            TransformDirection.Encrypt => _lifetimeScope.Resolve<DesBlockEncryptTransform>(
                new TypedParameter(typeof(IDesKey), key)
            ),
            TransformDirection.Decrypt => _lifetimeScope.Resolve<DesBlockDecryptTransform>(
                new TypedParameter(typeof(IDesKey), key)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported transform direction.")
        };
    }
}