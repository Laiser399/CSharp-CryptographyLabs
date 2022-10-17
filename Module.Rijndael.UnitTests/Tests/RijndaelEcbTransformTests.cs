using System.Security.Cryptography;
using Autofac;
using Module.Core.Enums;
using Module.Core.Services;
using Module.Core.Services.Abstract;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.UnitTests.Modules;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelEcbTransformTests
{
    private static readonly byte[] KeyBytes =
    {
        93, 230, 245, 204, 216, 129,
        66, 132, 92, 63, 19, 244,
        227, 157, 177, 22, 49, 56,
        50, 142, 121, 181, 247, 18
    };

    private static readonly RijndaelSize BlockSize = RijndaelSize.S192;

    private readonly IContainer _container;
    private readonly IRijndaelParameters _rijndaelParameters;
    private readonly Random _random = new();

    public RijndaelEcbTransformTests()
    {
        _container = BuildContainer();

        var key = _container
            .Resolve<IRijndaelKeyFactory>()
            .Create(KeyBytes);
        _rijndaelParameters = _container
            .Resolve<IRijndaelParametersFactory>()
            .Create(key, BlockSize);
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(10, 0)]
    [TestCase(999, 0)]
    [TestCase(999, 1)]
    [TestCase(999, 2)]
    [TestCase(999, 7)]
    [TestCase(999, -1)]
    public void Transform_Test(int blockCount, int handingByteCount)
    {
        var encryptTransform = CreateEncryptTransform();
        var decryptTransform = CreateDecryptTransform();
        TestTransform(
            encryptTransform.InputBlockSize * blockCount + handingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    private ICryptoTransform CreateEncryptTransform()
    {
        var blockCryptoTransform = _container.ResolveKeyed<IBlockCryptoTransform>(
            TransformDirection.Encrypt,
            new TypedParameter(typeof(IRijndaelParameters), _rijndaelParameters)
        );
        return new EcbEncryptTransform(blockCryptoTransform);
    }

    private ICryptoTransform CreateDecryptTransform()
    {
        var blockCryptoTransform = _container.ResolveKeyed<IBlockCryptoTransform>(
            TransformDirection.Decrypt,
            new TypedParameter(typeof(IRijndaelParameters), _rijndaelParameters)
        );
        return new EcbDecryptTransform(blockCryptoTransform);
    }

    private void TestTransform(int byteCount, ICryptoTransform encryptTransform, ICryptoTransform decryptTransform)
    {
        var data = new byte[byteCount];
        _random.NextBytes(data);

        var encrypted = Transform(data, encryptTransform);
        var decrypted = Transform(encrypted, decryptTransform);

        CollectionAssert.AreNotEqual(data, encrypted);
        CollectionAssert.AreEqual(data, decrypted);
    }

    private static byte[] Transform(byte[] data, ICryptoTransform cryptoTransform)
    {
        using var output = new MemoryStream();
        using var input = new MemoryStream(data);
        using var transformStream = new CryptoStream(input, cryptoTransform, CryptoStreamMode.Read);

        transformStream.CopyTo(output);
        return output.ToArray();
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<RijndaelModuleForTests>();

        return builder.Build();
    }
}