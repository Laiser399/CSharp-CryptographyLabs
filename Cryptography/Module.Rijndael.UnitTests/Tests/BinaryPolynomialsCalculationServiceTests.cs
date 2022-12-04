using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

public class BinaryPolynomialsCalculationServiceTests
{
    private IBinaryPolynomialsCalculationService? _binaryPolynomialsCalculationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _binaryPolynomialsCalculationService = new BinaryPolynomialsCalculationService();
    }

    [Test]
    [TestCase(0u, 0u, ExpectedResult = 0ul)]
    [TestCase(0b10000101u, 0b100010u, ExpectedResult = 0b1000110101010ul)]
    [TestCase(0b1011u, 0b10111u, ExpectedResult = 0b10000001ul)]
    public ulong Test(uint a, uint b)
    {
        return _binaryPolynomialsCalculationService!.Multiply(a, b);
    }
}