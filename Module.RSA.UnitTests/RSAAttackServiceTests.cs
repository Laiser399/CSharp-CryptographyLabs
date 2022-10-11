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
        _rsaAttackService = new RSAFactorizationAttackService(new BigIntegerCalculationService());
    }

    [Test]
    [TestCase("2118100319", "65537", "34450673")]
    [TestCase("111309534155653", "65537", "107803968648065")]
    public async Task Attack_TestAsync(
        string modulusStr,
        string publicExponentStr,
        string expectedPrivateExponentStr)
    {
        var modulus = BigInteger.Parse(modulusStr);
        var publicExponent = BigInteger.Parse(publicExponentStr);
        var expectedPrivateExponent = BigInteger.Parse(expectedPrivateExponentStr);

        var actualPrivateExponent = await _rsaAttackService!.AttackAsync(publicExponent, modulus);

        Assert.AreEqual(expectedPrivateExponent, actualPrivateExponent);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-928734)]
    public void Attack_InvalidPublicExponentTest(int publicExponent)
    {
        Assert.ThrowsAsync<ArgumentException>(
            () => _rsaAttackService!.AttackAsync(publicExponent, 2118100319)
        );
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-3295634)]
    public void Attack_InvalidModulusTest(int modulus)
    {
        Assert.ThrowsAsync<ArgumentException>(
            () => _rsaAttackService!.AttackAsync(65537, modulus)
        );
    }

    [Test]
    [TestCase(4937)]
    [TestCase(933931)]
    [TestCase(66476491)]
    public void Attack_PrimeTest(int modulus)
    {
        Assert.ThrowsAsync<CryptographyAttackException>(
            async () => await _rsaAttackService!.AttackAsync(65537, modulus)
        );
    }
}