using System.Security.Cryptography;
using Autofac;
using Module.Core.Enums;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.UnitTests.Modules;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelTransformsTests
{
    private static readonly byte[] KeyBytes =
    {
        93, 230, 245, 204, 216, 129,
        66, 132, 92, 63, 19, 244,
        227, 157, 177, 22, 49, 56,
        50, 142, 121, 181, 247, 18
    };

    private static readonly RijndaelSize BlockSize = RijndaelSize.S192;

    private static readonly byte[] InitialVector =
    {
        249, 215, 150, 208, 191, 44,
        178, 243, 120, 245, 137, 203,
        164, 21, 225, 170, 222, 19,
        75, 28, 217, 208, 125, 192
    };

    private readonly IRijndaelCryptoTransformFactory _rijndaelCryptoTransformFactory;
    private readonly Random _random = new();

    public RijndaelTransformsTests()
    {
        var container = BuildContainer();
        _rijndaelCryptoTransformFactory = container.Resolve<IRijndaelCryptoTransformFactory>();
    }

    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void Transform_Test(BlockCipherMode? mode, int blockCount, int handingByteCount)
    {
        var encryptTransform = GetCryptoTransform(mode, TransformDirection.Encrypt);
        var decryptTransform = GetCryptoTransform(mode, TransformDirection.Decrypt);

        TestTransform(
            encryptTransform.InputBlockSize * blockCount + handingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    private ICryptoTransform GetCryptoTransform(BlockCipherMode? mode, TransformDirection direction)
    {
        return mode switch
        {
            null => _rijndaelCryptoTransformFactory.CreateECB(direction, KeyBytes, BlockSize),
            _ => _rijndaelCryptoTransformFactory.Create(
                mode.Value,
                direction,
                InitialVector,
                KeyBytes,
                BlockSize
            )
        };
    }

    private static IReadOnlyCollection<object[]> GetTestCases()
    {
        var modes = new[]
        {
            (BlockCipherMode?)null,
            BlockCipherMode.CBC,
            BlockCipherMode.CFB,
            BlockCipherMode.OFB
        };
        var blockCounts = new[]
        {
            0, 10, 999
        };
        var handingByteCounts = new[]
        {
            0, 1, 2, -1, 7
        };

        var testCases =
            from mode in modes
            from blockCount in blockCounts
            from handingByteCount in handingByteCounts
            where blockCount != 0 || handingByteCount >= 0
            select new object[] { mode, blockCount, handingByteCount };

        return testCases.ToList();
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