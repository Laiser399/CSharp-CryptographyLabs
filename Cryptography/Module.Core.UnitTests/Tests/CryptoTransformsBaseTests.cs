using System.Security.Cryptography;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using NUnit.Framework;

// ReSharper disable StaticMemberInGenericType

namespace Module.Core.UnitTests.Tests;

public abstract class CryptoTransformsBaseTests<T>
{
    protected static readonly IReadOnlyCollection<BlockCipherMode> ModeCases = new[]
    {
        BlockCipherMode.CBC,
        BlockCipherMode.CFB,
        BlockCipherMode.OFB
    };

    protected static readonly IReadOnlyCollection<int> BlockCountCases = new[]
    {
        0, 10, 999
    };

    protected static readonly IReadOnlyCollection<int> HangingByteCountCases = new[]
    {
        0, 1, 2, -1, 7
    };

    private ICryptoTransformFactory<T>? _cryptoTransformFactory;

    private Random? _random;

    [SetUp]
    public void SetUp()
    {
        _random = new Random(123);
        _cryptoTransformFactory = GetCryptoTransformFactory();
    }

    protected abstract ICryptoTransformFactory<T> GetCryptoTransformFactory();

    protected void TestEcbTransform(
        T parameters,
        bool withParallelism,
        int blockCount,
        int hangingByteCount)
    {
        var encryptTransform = _cryptoTransformFactory!.CreateEcb(
            TransformDirection.Encrypt, parameters, withParallelism
        );
        var decryptTransform = _cryptoTransformFactory!.CreateEcb(
            TransformDirection.Decrypt, parameters, withParallelism
        );

        TestTransform(
            encryptTransform.InputBlockSize * blockCount + hangingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    protected void TestTransform(
        T parameters,
        BlockCipherMode mode,
        byte[] initialVector,
        int blockCount,
        int hangingByteCount)
    {
        var encryptTransform = _cryptoTransformFactory!.Create(
            TransformDirection.Encrypt, parameters, mode, initialVector
        );
        var decryptTransform = _cryptoTransformFactory!.Create(
            TransformDirection.Decrypt, parameters, mode, initialVector
        );

        TestTransform(
            encryptTransform.InputBlockSize * blockCount + hangingByteCount,
            encryptTransform,
            decryptTransform
        );
    }

    private void TestTransform(int byteCount, ICryptoTransform encryptTransform, ICryptoTransform decryptTransform)
    {
        var data = new byte[byteCount];
        _random!.NextBytes(data);

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
}