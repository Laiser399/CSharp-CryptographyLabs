using System.Numerics;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;
using NUnit.Framework;

namespace Module.RSA.UnitTests;

[TestFixture]
public class ContinuedFractionServiceTests
{
    private IContinuedFractionService? _continuedFractionService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _continuedFractionService = new ContinuedFractionService();
    }

    [Test]
    [TestCase("1", "1", new[] { "1" })]
    [TestCase("7", "7", new[] { "1" })]
    [TestCase("3", "37", new[] { "0", "12", "3" })]
    [TestCase("71", "17", new[] { "4", "5", "1", "2" })]
    [TestCase("37", "60", new[] { "0", "1", "1", "1", "1", "1", "1", "4" })]
    public void EnumerateContinuedFraction_Test(
        string numeratorStr,
        string denominatorStr,
        IEnumerable<string> expectedContinuedFractionStr)
    {
        var numerator = BigInteger.Parse(numeratorStr);
        var denominator = BigInteger.Parse(denominatorStr);
        var expectedContinuedFraction = expectedContinuedFractionStr.Select(BigInteger.Parse);

        var actualContinuedFraction = _continuedFractionService!.EnumerateContinuedFraction(numerator, denominator);

        CollectionAssert.AreEqual(expectedContinuedFraction, actualContinuedFraction);
    }

    [Test]
    [TestCase(-1, 3)]
    [TestCase(-378, 3)]
    [TestCase(3, 0)]
    [TestCase(3, -5)]
    public void EnumerateContinuedFraction_InvalidArgumentTest(int numerator, int denominator)
    {
        Assert.Throws<ArgumentException>(() =>
            _continuedFractionService!.EnumerateContinuedFraction(numerator, denominator)
        );
    }
}