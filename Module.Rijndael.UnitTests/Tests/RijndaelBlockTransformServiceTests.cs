using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelBlockTransformServiceTests
{
    private IRijndaelBlockTransformService? _rijndaelBlockTransformService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rijndaelBlockTransformService = null;
        throw new NotImplementedException();
    }

    [Test]
    public void Transform_Test()
    {
        var random = new Random(123);

        var text = new byte[16];
        var encrypted = new byte[16];
        var decrypted = new byte[16];
        for (var i = 0; i < 1000; i++)
        {
            random.NextBytes(text);
            _rijndaelBlockTransformService!.Encrypt(text, encrypted);
            _rijndaelBlockTransformService!.Decrypt(text, decrypted);

            CollectionAssert.AreNotEqual(text, encrypted);
            CollectionAssert.AreEqual(text, decrypted);
        }
    }

    [Test]
    public void Transform_InvalidArgumentTest()
    {
        Assert.Throws<ArgumentException>(
            () => _rijndaelBlockTransformService!.Encrypt(new byte[15], new byte[16])
        );
        Assert.Throws<ArgumentException>(
            () => _rijndaelBlockTransformService!.Encrypt(new byte[16], new byte[15])
        );
    }
}