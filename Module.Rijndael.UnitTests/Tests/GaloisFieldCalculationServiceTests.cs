using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using Module.Rijndael.UnitTests.Entities;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class GaloisFieldCalculationServiceTests
{
    private IGaloisFieldCalculationService? _galoisFieldCalculationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _galoisFieldCalculationService = new GaloisFieldCalculationService(new GaloisFieldConfigurationForTests(0b100011011));
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
    public void Inverse_FullTest()
    {
        var actual = _galoisFieldCalculationService!.Inverse(0);
        Assert.AreEqual(0, actual);

        for (var i = 1; i < 256; i++)
        {
            var a = (byte)i;
            var inversed = _galoisFieldCalculationService!.Inverse(a);

            Assert.AreEqual(1, _galoisFieldCalculationService!.Multiply(a, inversed));
            Assert.AreEqual(1, _galoisFieldCalculationService!.Multiply(inversed, a));
        }
    }
}