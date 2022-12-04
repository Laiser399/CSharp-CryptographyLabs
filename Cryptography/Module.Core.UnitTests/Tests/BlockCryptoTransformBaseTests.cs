using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using NUnit.Framework;

namespace Module.Core.UnitTests.Tests;

[TestFixture]
public abstract class BlockCryptoTransformBaseTests<T>
{
    private IBlockCryptoTransformFactory<T>? _blockCryptoTransformFactory;

    private Random? _random;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _blockCryptoTransformFactory = GetBlockCryptoTransformFactory();
    }

    [SetUp]
    public void SetUp()
    {
        _random = new Random(123);
    }

    protected abstract IBlockCryptoTransformFactory<T> GetBlockCryptoTransformFactory();

    protected void TestTransform(T parameters)
    {
        var blockEncryptTransform = _blockCryptoTransformFactory!.Create(TransformDirection.Encrypt, parameters);
        var blockDecryptTransform = _blockCryptoTransformFactory!.Create(TransformDirection.Decrypt, parameters);

        var data = new byte[blockEncryptTransform.InputBlockSize];
        var encrypted = new byte[blockEncryptTransform.OutputBlockSize];
        var decrypted = new byte[blockDecryptTransform.OutputBlockSize];

        for (var i = 0; i < 1000; i++)
        {
            _random!.NextBytes(data);
            blockEncryptTransform.Transform(data, encrypted);
            blockDecryptTransform.Transform(encrypted, decrypted);

            CollectionAssert.AreNotEqual(data, encrypted);
            CollectionAssert.AreEqual(data, decrypted);
        }
    }

    [Test]
    public void Transform_InvalidArgumentTest()
    {
        var parameters = GetDefaultParameters();
        var blockEncryptTransform = _blockCryptoTransformFactory!.Create(TransformDirection.Encrypt, parameters);
        var blockDecryptTransform = _blockCryptoTransformFactory!.Create(TransformDirection.Decrypt, parameters);

        Assert.Throws<ArgumentException>(
            () => blockEncryptTransform.Transform(
                new byte[blockEncryptTransform.InputBlockSize - 1],
                new byte[blockEncryptTransform.OutputBlockSize]
            )
        );
        Assert.Throws<ArgumentException>(
            () => blockEncryptTransform.Transform(
                new byte[blockEncryptTransform.InputBlockSize],
                new byte[blockEncryptTransform.OutputBlockSize - 1]
            )
        );
        Assert.Throws<ArgumentException>(
            () => blockDecryptTransform.Transform(
                new byte[blockDecryptTransform.InputBlockSize - 1],
                new byte[blockDecryptTransform.OutputBlockSize]
            )
        );
        Assert.Throws<ArgumentException>(
            () => blockDecryptTransform.Transform(
                new byte[blockDecryptTransform.InputBlockSize],
                new byte[blockDecryptTransform.OutputBlockSize - 1]
            )
        );
    }

    protected abstract T GetDefaultParameters();
}