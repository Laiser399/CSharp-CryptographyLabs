using Module.Core.Services;
using Module.Core.Services.Abstract;
using NUnit.Framework;

namespace Module.Core.UnitTests.Tests;

[TestFixture]
public class XorServiceTests
{
    private IXorService? _xorService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _xorService = new XorService();
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(8)]
    [TestCase(999)]
    public void Test(int byteCount)
    {
        var random = new Random(123);
        var first = new byte[byteCount];
        var second = new byte[byteCount];
        random.NextBytes(first);
        random.NextBytes(second);

        var expected = new byte[byteCount];
        for (var i = 0; i < byteCount; i++)
        {
            expected[i] = (byte)(first[i] ^ second[i]);
        }

        var actual = new byte[byteCount];
        _xorService!.Xor(first, second, actual);

        CollectionAssert.AreEqual(expected, actual);
    }
}