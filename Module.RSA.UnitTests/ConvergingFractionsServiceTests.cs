using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Services.Abstract;
using NUnit.Framework;

namespace Module.RSA.UnitTests;

[TestFixture]
public class ConvergingFractionsServiceTests
{
    private IConvergingFractionsService? _convergingFractionsService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _convergingFractionsService = null;
        throw new NotImplementedException();
    }

    [Test]
    [TestCase(
        new[] { "1" },
        new[] { "1" },
        new[] { "1" }
    )]
    [TestCase(
        new[] { "0", "12", "3" },
        new[] { "0", "1", "3" },
        new[] { "1", "12", "37" }
    )]
    [TestCase(
        new[] { "4", "5", "1", "2" },
        new[] { "4", "21", "25", "71" },
        new[] { "1", "5", "6", "17" }
    )]
    public void EnumerateConvergingFractions_Test(
        IEnumerable<string> continuedFractionStr,
        IEnumerable<string> expectedConvergingFractionNumeratorsStr,
        IEnumerable<string> expectedConvergingFractionDenominatorsStr)
    {
        var continuedFraction = continuedFractionStr.Select(BigInteger.Parse);
        var expectedConvergingFractions = expectedConvergingFractionNumeratorsStr
            .Zip(expectedConvergingFractionDenominatorsStr)
            .Select(x => new ConvergingFraction(BigInteger.Parse(x.First), BigInteger.Parse(x.Second)));

        var actualConvergingFractions = _convergingFractionsService!.EnumerateConvergingFractions(continuedFraction);

        CollectionAssert.AreEqual(expectedConvergingFractions, actualConvergingFractions);
    }

    [Test]
    public void EnumerateConvergingFractions_EmptyArgumentTest()
    {
        var convergingFractions = _convergingFractionsService!.EnumerateConvergingFractions(Array.Empty<BigInteger>());
        CollectionAssert.IsEmpty(convergingFractions);
    }

    [Test]
    public void EnumerateConvergingFractions_InvalidArgumentTest()
    {
        Assert.Throws<ArgumentException>(() =>
            _convergingFractionsService!.EnumerateConvergingFractions(new BigInteger[] { -1 })
        );
        Assert.Throws<ArgumentException>(() =>
            _convergingFractionsService!.EnumerateConvergingFractions(new BigInteger[] { 1, 0, 3, -37 })
        );
        Assert.Throws<ArgumentException>(() =>
            _convergingFractionsService!.EnumerateConvergingFractions(new BigInteger[] { 7, 7, -9, 7, 7 })
        );
    }
}