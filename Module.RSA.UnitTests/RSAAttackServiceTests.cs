using System.Numerics;
using Module.RSA.Exceptions;
using Module.RSA.Services;
using Module.RSA.Services.Abstract;
using NUnit.Framework;

namespace Module.RSA.UnitTests;

[TestFixture]
public class RSAAttackServiceTests
{
    private IRSAAttackService? _rsaAttackService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rsaAttackService = new RSAAttackService();
    }

    [Test]
    [TestCase("9", "3", "3")]
    [TestCase("15", "3", "5")]
    [TestCase("2118100319", "35869", "59051")]
    [TestCase("111309534155653", "8575969", "12979237")]
    public async Task FactorizeModulus_TestAsync(
        string modulusStr,
        string expectedLowerFactorStr,
        string expectedHigherFactorStr)
    {
        var modulus = BigInteger.Parse(modulusStr);
        var expectedLowerFactor = BigInteger.Parse(expectedLowerFactorStr);
        var expectedHigherFactor = BigInteger.Parse(expectedHigherFactorStr);

        var factorizationResult = await _rsaAttackService!.FactorizeModulusAsync(modulus);

        Assert.AreEqual(expectedLowerFactor, factorizationResult.LowerFactor);
        Assert.AreEqual(expectedHigherFactor, factorizationResult.HigherFactor);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-928734)]
    public void FactorizeModulus_InvalidArgumentTest(int modulus)
    {
        Assert.ThrowsAsync<ArgumentException>(
            async () => await _rsaAttackService!.FactorizeModulusAsync(modulus)
        );
    }

    [Test]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(17)]
    [TestCase(35869)]
    public void FactorizeModulus_PrimeTest(int modulus)
    {
        Assert.ThrowsAsync<FactorizationException>(
            async () => await _rsaAttackService!.FactorizeModulusAsync(modulus)
        );
    }
}