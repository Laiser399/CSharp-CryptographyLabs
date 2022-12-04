using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelShiftRowsServiceTests
{
    private IRijndaelShiftRowsService? _rijndaelShiftRowsService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rijndaelShiftRowsService = new RijndaelShiftRowsService();
    }

    [Test]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        },
        new byte[]
        {
            1, 2, 3, 4,
            6, 7, 8, 5,
            11, 12, 9, 10,
            16, 13, 14, 15
        }
    )]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12,
            13, 14, 15, 16, 17, 18,
            19, 20, 21, 22, 23, 24
        },
        new byte[]
        {
            1, 2, 3, 4, 5, 6,
            8, 9, 10, 11, 12, 7,
            15, 16, 17, 18, 13, 14,
            22, 23, 24, 19, 20, 21
        }
    )]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24,
            25, 26, 27, 28, 29, 30, 31, 32
        },
        new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            10, 11, 12, 13, 14, 15, 16, 9,
            19, 20, 21, 22, 23, 24, 17, 18,
            28, 29, 30, 31, 32, 25, 26, 27
        }
    )]
    public void ShiftRows_Test(byte[] state, byte[] expected)
    {
        _rijndaelShiftRowsService!.ShiftRows(state);
        CollectionAssert.AreEqual(expected, state);
    }

    [Test]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        },
        new byte[]
        {
            1, 2, 3, 4,
            8, 5, 6, 7,
            11, 12, 9, 10,
            14, 15, 16, 13
        }
    )]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4, 5, 6,
            7, 8, 9, 10, 11, 12,
            13, 14, 15, 16, 17, 18,
            19, 20, 21, 22, 23, 24
        },
        new byte[]
        {
            1, 2, 3, 4, 5, 6,
            12, 7, 8, 9, 10, 11,
            17, 18, 13, 14, 15, 16,
            22, 23, 24, 19, 20, 21
        }
    )]
    [TestCase(
        new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24,
            25, 26, 27, 28, 29, 30, 31, 32
        },
        new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            16, 9, 10, 11, 12, 13, 14, 15,
            23, 24, 17, 18, 19, 20, 21, 22,
            30, 31, 32, 25, 26, 27, 28, 29
        }
    )]
    public void InverseShiftRows_Test(byte[] state, byte[] expected)
    {
        _rijndaelShiftRowsService!.InverseShiftRows(state);
        CollectionAssert.AreEqual(expected, state);
    }

    [Test]
    [TestCase(new byte[] { })]
    [TestCase(new byte[] { 42 })]
    [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })]
    [TestCase(new byte[]
    {
        1, 2, 3, 4, 5, 6, 7, 8,
        9, 10, 11, 12, 13, 14, 15, 16,
        17, 18, 19, 20, 21, 22, 23, 24,
        25, 26, 27, 28, 29, 30, 31, 32,
        33
    })]
    public void Common_InvalidArgumentTest(byte[] state)
    {
        Assert.Throws<ArgumentException>(
            () => _rijndaelShiftRowsService!.ShiftRows(state)
        );
        Assert.Throws<ArgumentException>(
            () => _rijndaelShiftRowsService!.InverseShiftRows(state)
        );
    }
}