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
    [TestCase(7, 0, 7)]
    [TestCase(0, 5, 5)]
    [TestCase(1, 1, 1)]
    [TestCase(1, 3, 1)]
    [TestCase(3, 1, 1)]
    [TestCase(49, 7, 7)]
    [TestCase(7, 49, 7)]
    [TestCase(14, 21, 7)]
    [TestCase(21, 14, 7)]
    [TestCase(1024, 1024, 1024)]
    [TestCase(5 * 13 * 17 * 17, 3 * 5 * 17, 5 * 17)]
    [TestCase(3 * 5 * 17, 5 * 13 * 17 * 17, 5 * 17)]
    public void GreatestCommonDivisor_Test(int a, int b, int expectedGCD)
    {
        var actualGCD1 = _bigIntegerCalculationService!.GreatestCommonDivisor(a, b);
        var actualGCD2 = _bigIntegerCalculationService!.GreatestCommonDivisor(a, b, out var x, out var y);

        Assert.AreEqual((BigInteger)expectedGCD, actualGCD1);
        Assert.AreEqual((BigInteger)expectedGCD, actualGCD2);
        Assert.AreEqual(actualGCD2, x * a + y * b);
    }

    [Test]
    [TestCase("0", "0")]
    [TestCase("1", "1")]
    [TestCase("4", "2")]
    [TestCase("16", "4")]
    [TestCase("13")]
    [TestCase("51")]
    [TestCase("8887348", "2981")]
    [TestCase("5867945618446744073709551615")]
    [TestCase("410529583717306063201", "20261529649")]
    [TestCase("29348752039485723094586213045976134059826345039274865203847565654")]
    public void SquareRoot_Test(string valueStr, string? expectedExactResultStr = null)
    {
        var value = BigInteger.Parse(valueStr);
        var expectedExactResult = expectedExactResultStr is not null
            ? BigInteger.Parse(expectedExactResultStr)
            : (BigInteger?)null;
        var actualResult = _bigIntegerCalculationService!.SquareRoot(value);

        if (expectedExactResult is not null)
        {
            Assert.AreEqual((BigInteger)expectedExactResult, actualResult);
        }
        else
        {
            var leftBound = actualResult * actualResult;
            var rightBound = (actualResult + 1) * (actualResult + 1);
            Assert.GreaterOrEqual(value, leftBound);
            Assert.LessOrEqual(value, rightBound);
        }
    }

    [Test]
    [TestCase("0", 0)]
    [TestCase("1", 1)]
    [TestCase("4")]
    [TestCase("13")]
    [TestCase("16", 2)]
    [TestCase("49")]
    [TestCase("8887348", 54)]
    [TestCase("5867945618446744073709551615")]
    [TestCase("410529583717306063201", 142343)]
    public void FourthRoot_Test(string valueStr, int? expectedExactResult = null)
    {
        var value = BigInteger.Parse(valueStr);
        var actualResult = _bigIntegerCalculationService!.FourthRoot(value);

        if (expectedExactResult is not null)
        {
            Assert.AreEqual((BigInteger)expectedExactResult, actualResult);
        }
        else
        {
            var leftBound = BigInteger.Pow(actualResult, 4);
            var rightBound = BigInteger.Pow(actualResult + 1, 4);
            Assert.GreaterOrEqual(value, leftBound);
            Assert.LessOrEqual(value, rightBound);
        }
    }
}