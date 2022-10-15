using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class GaloisFieldCalculationServiceTests
{
    private IGaloisFieldCalculationService? _galoisFieldCalculationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _galoisFieldCalculationService = null;
        throw new NotImplementedException();
    }

    [Test]
    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(1, 0b1000, 0b1000)]
    [TestCase(0b1001, 0b1101, 0b1100101)]
    [TestCase(0b1001101, 0b1100110, 0b10000)]
    [TestCase(0b111, 0b1001101, 0b11111000)]
    public void Multiply_Test(byte a, byte b, byte expected)
    {
        var actual = _galoisFieldCalculationService!.Multiply(a, b);
        Assert.AreEqual(expected, actual);
    }

    // todo add cases later
    [Test]
    [TestCase(0, 0, 0)]
    public void Divide_Test(byte a, byte b, byte expected)
    {
        var actual = _galoisFieldCalculationService!.Divide(a, b);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(1, 1)]
    public void Inverse_Test(byte a, byte expected)
    {
        var actual = _galoisFieldCalculationService!.Inverse(a);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [TestCase(0b1001)]
    [TestCase(0b1001101)]
    [TestCase(0b11111000)]
    [TestCase(0b101010)]
    [TestCase(0b1100011)]
    public void Inverse_Test(byte a)
    {
        var inversed = _galoisFieldCalculationService!.Inverse(a);

        Assert.AreEqual(1, _galoisFieldCalculationService!.Multiply(a, inversed));
        Assert.AreEqual(1, _galoisFieldCalculationService!.Multiply(inversed, a));
    }
}