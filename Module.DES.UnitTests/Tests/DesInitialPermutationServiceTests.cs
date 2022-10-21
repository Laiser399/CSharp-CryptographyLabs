using Module.DES.Services;
using Module.DES.Services.Abstract;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class DesInitialPermutationServiceTests
{
    private static readonly byte[] PermutationTable =
    {
        57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
        56, 48, 40, 32, 24, 16, 8, 0, 58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6
    };

    private IUInt64BitPermutationService? _uInt64BitPermutationService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _uInt64BitPermutationService = new DesInitialPermutationService();
    }

    [Test]
    public void Permute_Test()
    {
        for (var i = 0; i < 64; i++)
        {
            var value = 1ul << i;
            var expected = PermuteWithTable(value);
            var actual = _uInt64BitPermutationService!.Permute(value);
            Assert.AreEqual(expected, actual);
        }
    }

    private static ulong PermuteWithTable(ulong value)
    {
        var result = 0ul;
        for (var i = 0; i < PermutationTable.Length; i++)
        {
            result |= ((value >> PermutationTable[i]) & 1) << i;
        }

        return result;
    }
}