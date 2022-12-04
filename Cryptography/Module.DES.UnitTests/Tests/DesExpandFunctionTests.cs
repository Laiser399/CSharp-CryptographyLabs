using Module.DES.Services;
using Module.DES.Services.Abstract;
using Module.DES.UnitTests.Helpers;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class DesExpandFunctionTests
{
    private static readonly IReadOnlyList<byte> PermutationTable = new byte[]
    {
        31, 0, 1, 2, 3, 4,
        3, 4, 5, 6, 7, 8,
        7, 8, 9, 10, 11, 12,
        11, 12, 13, 14, 15, 16,
        15, 16, 17, 18, 19, 20,
        19, 20, 21, 22, 23, 24,
        23, 24, 25, 26, 27, 28,
        27, 28, 29, 30, 31, 0,
    };

    private IDesExpandFunction? _desExpandFunction;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _desExpandFunction = new DesExpandFunction();
    }

    [Test]
    [TestCase(
        0b01111001_01011011_00000110_10110110u,
        ExpectedResult = 0b001111_110010_101011_110110_100000_001101_010110_101100ul
    )]
    public ulong Calculate_Test(uint value)
    {
        return _desExpandFunction!.Calculate(value);
    }

    [Test]
    public void Calculate_PermutationTest()
    {
        for (var i = 0; i < 32; i++)
        {
            var value = 1u << i;
            var expected = PermutationHelper.PermuteWithTable((ulong)value, PermutationTable);
            var actual = _desExpandFunction!.Calculate(value);

            Assert.AreEqual(expected, actual);
        }
    }
}