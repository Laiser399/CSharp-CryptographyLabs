using System.Numerics;
using Autofac;
using Module.RSA.Services.Abstract;
using Module.RSA.UnitTests.Modules;
using NUnit.Framework;

namespace Module.RSA.UnitTests.Tests;

[TestFixture]
public class FactorizationAttackServiceTests
{
    private readonly IContainer _container;

    private IRSAAttackService? _rsaAttackService;

    public FactorizationAttackServiceTests()
    {
        _container = BuildContainer();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rsaAttackService = _container.Resolve<IRSAAttackService>();
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

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<FactorizationAttackModule>();

        return builder.Build();
    }
}