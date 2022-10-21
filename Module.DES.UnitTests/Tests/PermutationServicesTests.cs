using Module.DES.Services;
using Module.DES.Services.Abstract;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class PermutationServicesTests
{
    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void Permute_Test(
        IUInt64BitPermutationService permutationService,
        IReadOnlyList<byte> permutationTable)
    {
        for (var i = 0; i < permutationTable.Count; i++)
        {
            var value = 1ul << i;
            var expected = PermuteWithTable(value, permutationTable);
            var actual = permutationService.Permute(value);
            Assert.AreEqual(expected, actual);
        }
    }

    private static ulong PermuteWithTable(ulong value, IReadOnlyList<byte> permutationTable)
    {
        var result = 0ul;
        for (var i = 0; i < permutationTable.Count; i++)
        {
            result |= ((value >> permutationTable[i]) & 1) << i;
        }

        return result;
    }

    private static IEnumerable<object[]> GetTestCases()
    {
        var bitPermutationService = new BitPermutationService();

        var initialPermutationTable = new byte[]
        {
            57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
            56, 48, 40, 32, 24, 16, 8, 0, 58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6
        };
        yield return new object[]
        {
            new DesInitialPermutationService(bitPermutationService),
            initialPermutationTable
        };

        var finalPermutationTable = new byte[]
        {
            39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25, 32, 0, 40, 8, 48, 16, 56, 24,
        };
        yield return new object[]
        {
            new DesFinalPermutationService(),
            finalPermutationTable
        };
    }
}