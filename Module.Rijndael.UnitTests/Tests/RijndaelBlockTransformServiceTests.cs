using Autofac;
using Module.Core.Enums;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.UnitTests.Modules;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelBlockTransformServiceTests
{
    private static readonly IReadOnlyCollection<RijndaelSize> BlockSizes = new[]
    {
        RijndaelSize.S128,
        RijndaelSize.S192,
        RijndaelSize.S256
    };

    private IRijndaelKeyFactory? _rijndaelKeyFactory;
    private IRijndaelParametersFactory? _rijndaelParametersFactory;
    private Func<IRijndaelParameters, IBlockCryptoTransform>? _blockEncryptTransformFactory;
    private Func<IRijndaelParameters, IBlockCryptoTransform>? _blockDecryptTransformFactory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var container = BuildContainer();
        _rijndaelKeyFactory = container.Resolve<IRijndaelKeyFactory>();
        _rijndaelParametersFactory = container.Resolve<IRijndaelParametersFactory>();
        _blockEncryptTransformFactory =
            container.ResolveKeyed<Func<IRijndaelParameters, IBlockCryptoTransform>>(TransformDirection.Encrypt);
        _blockDecryptTransformFactory =
            container.ResolveKeyed<Func<IRijndaelParameters, IBlockCryptoTransform>>(TransformDirection.Encrypt);
    }

    [Test]
    [TestCase(
        new byte[]
        {
            220, 98, 68, 222,
            197, 55, 127, 233,
            53, 242, 68, 32,
            91, 119, 175, 95
        }
    )]
    [TestCase(
        new byte[]
        {
            93, 230, 245, 204, 216, 129,
            66, 132, 92, 63, 19, 244,
            227, 157, 177, 22, 49, 56,
            50, 142, 121, 181, 247, 18
        }
    )]
    [TestCase(
        new byte[]
        {
            130, 237, 235, 12, 213, 170, 49, 57,
            87, 227, 107, 115, 51, 119, 87, 130,
            246, 189, 57, 255, 113, 65, 74, 72,
            244, 66, 191, 162, 9, 233, 234, 1
        }
    )]
    public void Transform_Test(byte[] keyBytes)
    {
        var random = new Random(123);

        foreach (var blockSize in BlockSizes)
        {
            var key = _rijndaelKeyFactory!.Create(keyBytes);
            var parameters = _rijndaelParametersFactory!.Create(key, blockSize);
            var blockEncryptTransform = _blockEncryptTransformFactory!(parameters);
            var blockDecryptTransform = _blockDecryptTransformFactory!(parameters);

            var text = new byte[blockSize.ByteCount];
            var encrypted = new byte[blockSize.ByteCount];
            var decrypted = new byte[blockSize.ByteCount];

            for (var i = 0; i < 1000; i++)
            {
                random.NextBytes(text);
                blockEncryptTransform.Transform(text, encrypted);
                blockDecryptTransform.Transform(encrypted, decrypted);

                CollectionAssert.AreNotEqual(text, encrypted);
                CollectionAssert.AreEqual(text, decrypted);
            }
        }
    }

    [Test]
    public void Transform_InvalidArgumentTest()
    {
        var keyBytes = new byte[]
        {
            220, 98, 68, 222,
            197, 55, 127, 233,
            53, 242, 68, 32,
            91, 119, 175, 95
        };
        var blockSize = RijndaelSize.S128;

        var key = _rijndaelKeyFactory!.Create(keyBytes);
        var parameters = _rijndaelParametersFactory!.Create(key, blockSize);
        var blockEncryptTransform = _blockEncryptTransformFactory!(parameters);
        var blockDecryptTransform = _blockDecryptTransformFactory!(parameters);

        Assert.Throws<ArgumentException>(
            () => blockEncryptTransform.Transform(new byte[15], new byte[16])
        );
        Assert.Throws<ArgumentException>(
            () => blockEncryptTransform.Transform(new byte[16], new byte[15])
        );
        Assert.Throws<ArgumentException>(
            () => blockDecryptTransform.Transform(new byte[15], new byte[16])
        );
        Assert.Throws<ArgumentException>(
            () => blockDecryptTransform.Transform(new byte[16], new byte[15])
        );
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<RijndaelModuleForTests>();

        return builder.Build();
    }
}