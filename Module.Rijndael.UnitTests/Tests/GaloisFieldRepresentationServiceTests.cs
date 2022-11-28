using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class GaloisFieldRepresentationServiceTests
{
    private IBinaryPolynomialRepresentationService? _galoisFieldRepresentationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _galoisFieldRepresentationService = new BinaryPolynomialRepresentationService();
    }

    [Test]
    [TestCase(0, ExpectedResult = "0")]
    [TestCase(0b1, ExpectedResult = "1")]
    [TestCase(0b11, ExpectedResult = "x + 1")]
    [TestCase(0b10, ExpectedResult = "x")]
    [TestCase(0b1001, ExpectedResult = "x^3 + 1")]
    [TestCase(0b110001, ExpectedResult = "x^5 + x^4 + 1")]
    public string ToString_Test(byte value)
    {
        return _galoisFieldRepresentationService!.ToString(value);
    }

    [Test]
    [TestCase(0u, ExpectedResult = "0")]
    [TestCase(0b1u, ExpectedResult = "1")]
    [TestCase(0b10000000_00000000_00000001_10000000u, ExpectedResult = "x^31 + x^8 + x^7")]
    public string ToString_Test(uint value)
    {
        return _galoisFieldRepresentationService!.ToString(value);
    }

    [Test]
    [TestCase("0", ExpectedResult = 0)]
    [TestCase("1 + 1", ExpectedResult = 0)]
    [TestCase(" 1 ", ExpectedResult = 0b1)]
    [TestCase("x^0", ExpectedResult = 0b1)]
    [TestCase("x", ExpectedResult = 0b10)]
    [TestCase("x^1", ExpectedResult = 0b10)]
    [TestCase("x^7", ExpectedResult = 0b10000000)]
    [TestCase("x^2 + 1", ExpectedResult = 0b101)]
    [TestCase("x^2 + x", ExpectedResult = 0b110)]
    [TestCase("x^2+x", ExpectedResult = 0b110)]
    [TestCase("x^2 +x", ExpectedResult = 0b110)]
    [TestCase("x^2 + 0", ExpectedResult = 0b100)]
    [TestCase("x^1 + x^2", ExpectedResult = 0b110)]
    [TestCase("x ^2", ExpectedResult = 0b100)]
    [TestCase("x^ 2", ExpectedResult = 0b100)]
    [TestCase("x^6 + x^6", ExpectedResult = 0)]
    [TestCase("x^6 + x^6 + x^1", ExpectedResult = 0b10)]
    public byte TryParse_ByteTest(string polynomial)
    {
        var parsed = _galoisFieldRepresentationService!.TryParse(polynomial, out byte value);
        Assert.IsTrue(parsed);
        return value;
    }

    [Test]
    [TestCase("x^28 + x^31", ExpectedResult = 0b10010000_00000000_00000000_00000000u)]
    public uint TryParse_UintTest(string polynomial)
    {
        var parsed = _galoisFieldRepresentationService!.TryParse(polynomial, out uint value);
        Assert.IsTrue(parsed);
        return value;
    }

    [Test]
    [TestCase("")]
    [TestCase("     ")]
    [TestCase("y")]
    [TestCase("3")]
    [TestCase("x^32")]
    [TestCase("x^2 - x")]
    [TestCase("x^2 + -x")]
    public void TryParse_InvalidArgumentTest(string polynomial)
    {
        var byteParsed = _galoisFieldRepresentationService!.TryParse(polynomial, out byte _);
        var uintParsed = _galoisFieldRepresentationService!.TryParse(polynomial, out uint _);
        Assert.IsFalse(byteParsed);
        Assert.IsFalse(uintParsed);
    }
}