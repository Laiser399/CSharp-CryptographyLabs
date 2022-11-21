using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class GaloisFieldRepresentationServiceTests
{
    private IGaloisFieldRepresentationService? _galoisFieldRepresentationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _galoisFieldRepresentationService = null;
        throw new NotImplementedException();
    }

    [Test]
    [TestCase(0, ExpectedResult = "0")]
    [TestCase(0b1, ExpectedResult = "1")]
    [TestCase(0b11, ExpectedResult = "x + 1")]
    [TestCase(0b10, ExpectedResult = "x")]
    [TestCase(0b1001, ExpectedResult = "x^3 + 1")]
    [TestCase(0b110001, ExpectedResult = "x^5 + x^4 + 1")]
    public string ToStringAsPolynomial_Test(byte value)
    {
        return _galoisFieldRepresentationService!.ToStringAsPolynomial(value);
    }

    [Test]
    [TestCase("0", ExpectedResult = 0)]
    [TestCase(" 1 ", ExpectedResult = 0b1)]
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
    public byte TryParseAsPolynomial_Test(string polynomial)
    {
        var parsed = _galoisFieldRepresentationService!.TryParseAsPolynomial(polynomial, out var value);
        Assert.IsTrue(parsed);
        return value;
    }

    [Test]
    [TestCase("")]
    [TestCase("     ")]
    [TestCase("y")]
    [TestCase("3")]
    [TestCase("x^8")]
    [TestCase("x^2 - x")]
    [TestCase("x^0")]
    [TestCase("x^2 + -x")]
    [TestCase("1 + 1")]
    [TestCase("x^2 + x^2")]
    public void TryParseAsPolynomial_InvalidArgumentTest(string polynomial)
    {
        var parsed = _galoisFieldRepresentationService!.TryParseAsPolynomial(polynomial, out _);
        Assert.IsFalse(parsed);
    }
}