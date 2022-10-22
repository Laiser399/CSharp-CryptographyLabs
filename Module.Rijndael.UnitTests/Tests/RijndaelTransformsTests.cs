using System.Security.Cryptography;
using Autofac;
using Module.Core.Enums;
using Module.Rijndael.Entities;
using Module.Rijndael.Enums;
using Module.Rijndael.UnitTests.Modules;
using NUnit.Framework;
using IRijndaelCryptoTransformFactory = Module.Core.Factories.Abstract.ICryptoTransformFactory<
    Module.Rijndael.Entities.Abstract.IRijndaelParameters
>;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelTransformsTests
{
    private static readonly IReadOnlyCollection<BlockCipherMode> ModeCases = new[]
    {
        BlockCipherMode.CBC,
        BlockCipherMode.CFB,
        BlockCipherMode.OFB
    };

    private static readonly IReadOnlyCollection<int> BlockCountCases = new[]
    {
        0, 10, 999
    };

    private static readonly IReadOnlyCollection<int> HangingByteCountCases = new[]
    {
        0, 1, 2, -1, 7
    };

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
    private Random _random = new(0);

    public RijndaelTransformsTests()
    {
        var container = BuildContainer();
        _rijndaelCryptoTransformFactory = container.Resolve<IRijndaelCryptoTransformFactory>();
    }

    [SetUp]
    public void SetUp()
    {
        _random = new Random(123);
    }

    [Test]
    [TestCaseSource(nameof(GetEcbTestCases))]
    public void Transform_EcbTest(int blockCount, int hangingByteCount, bool withParallelism)
    {
        var encryptTransform = GetEcbCryptoTransform(TransformDirection.Encrypt, withParallelism);
        var decryptTransform = GetEcbCryptoTransform(TransformDirection.Decrypt, withParallelism);

        TestTransform(
            encryptTransform.InputBlockSize * blockCount + hangingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void Transform_Test(BlockCipherMode mode, int blockCount, int hangingByteCount)
    {
        var encryptTransform = GetCryptoTransform(mode, TransformDirection.Encrypt);
        var decryptTransform = GetCryptoTransform(mode, TransformDirection.Decrypt);

        TestTransform(
            encryptTransform.InputBlockSize * blockCount + hangingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    private ICryptoTransform GetEcbCryptoTransform(TransformDirection direction, bool withParallelism)
    {
        return _rijndaelCryptoTransformFactory.CreateEcb(
            direction,
            new RijndaelParameters(KeyBytes, BlockSize),
            withParallelism
        );
    }

    private ICryptoTransform GetCryptoTransform(BlockCipherMode mode, TransformDirection direction)
    {
        return _rijndaelCryptoTransformFactory.Create(
            direction,
            new RijndaelParameters(KeyBytes, BlockSize),
            mode,
            InitialVector
        );
    }

    private static IReadOnlyCollection<object[]> GetEcbTestCases()
    {
        var testCases =
            from blockCount in BlockCountCases
            from hangingByteCount in HangingByteCountCases
            from withParallelism in new[] { true, false }
            where blockCount != 0 || hangingByteCount >= 0
            select new object[] { blockCount, hangingByteCount, withParallelism };

        return testCases.ToList();
    }

    private static IReadOnlyCollection<object[]> GetTestCases()
    {
        var testCases =
            from mode in ModeCases
            from blockCount in BlockCountCases
            from hangingByteCount in HangingByteCountCases
            where blockCount != 0 || hangingByteCount >= 0
            select new object[] { mode, blockCount, hangingByteCount };

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