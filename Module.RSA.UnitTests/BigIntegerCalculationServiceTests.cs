using System.Numerics;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;
using NUnit.Framework;

namespace Module.RSA.UnitTests;

[TestFixture]
public class BigIntegerCalculationServiceTests
{
    private IBigIntegerCalculationService? _bigIntegerCalculationService;

    [OneTimeSetUp]
    public void SetUp()
    {
        _bigIntegerCalculationService = new BigIntegerCalculationService();
    }

    [Test]
    [TestCase(0, 2, 3, 0)]
    [TestCase(1, 20, 3, 1)]
    [TestCase(2, 19, 7, 2)]
    [TestCase(7, 120, 13, 1)]
    public void BinPowMod_Test(int value, int pow, int mod, int expected)
    {
        var actual = _bigIntegerCalculationService!.BinPowMod(value, pow, mod);
        Assert.AreEqual((BigInteger)expected, actual);
    }

    [Test]
    [TestCase(1, 0, 1)]
    [TestCase(2, 1, 1)]
    [TestCase(13, 0, 13)]
    [TestCase(1024, 10, 1)]
    [TestCase(392, 3, 49)]
    public void Factor2Out_CommonTest(int value, int expectedExponent2, int expectedRemainder)
    {
        _bigIntegerCalculationService!.Factor2Out(value, out var actualExponent2, out var actualRemainder);

        Assert.AreEqual(expectedExponent2, actualExponent2);
        Assert.AreEqual((BigInteger)expectedRemainder, actualRemainder);
    }

    [Test, Timeout(1000)]
    public void Factor2Out_InvalidArgumentTest()
    {
        Assert.Throws<ArgumentException>(
            () => _bigIntegerCalculationService!.Factor2Out(0, out _, out _)
        );

        Assert.Throws<ArgumentException>(
            () => _bigIntegerCalculationService!.Factor2Out(-3, out _, out _)
        );
    }

    [Test]
    [TestCase(1, 1, 1)]
    [TestCase(1, 3, 1)]
    [TestCase(3, 1, 1)]
    [TestCase(49, 7, 7)]
    [TestCase(7, 49, 7)]
    [TestCase(14, 21, 7)]
    [TestCase(1024, 1024, 1024)]
    public void GreatestCommonDivisor_Test(int a, int b, int expected)
    {
        var actual = _bigIntegerCalculationService!.GreatestCommonDivisor(a, b);

        Assert.AreEqual((BigInteger)expected, actual);
    }
}