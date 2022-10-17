using Module.Rijndael.Factories;
using Module.Rijndael.Services;
using Module.Rijndael.Services.Abstract;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelMixColumnsServiceTests
{
    private IRijndaelMixColumnsService? _rijndaelMixColumnsService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _rijndaelMixColumnsService = new RijndaelMixColumnsService(
            new GaloisFieldCalculationService(
                GaloisFieldConfigurationFactory.DefaultConfiguration
            )
        );
    }

    [Test]
    [TestCase(
        new byte[]
        {
            69, 179, 140, 149, 25, 164,
            126, 213, 132, 32, 176, 14,
            158, 251, 110, 112, 123, 37,
            1, 242, 142, 201, 205, 12
        },
        new byte[]
        {
            151, 16, 116, 232, 79, 104,
            1, 230, 163, 140, 34, 219,
            31, 134, 93, 21, 19, 244,
            45, 31, 98, 125, 97, 196,
        }
    )]
    public void MixColumns_Test(byte[] state, byte[] expected)
    {
        _rijndaelMixColumnsService!.MixColumns(state);
        CollectionAssert.AreEqual(expected, state);
    }

    [Test]
    [TestCase(
        new byte[]
        {
            151, 16, 116, 232, 79, 104,
            1, 230, 163, 140, 34, 219,
            31, 134, 93, 21, 19, 244,
            45, 31, 98, 125, 97, 196,
        },
        new byte[]
        {
            69, 179, 140, 149, 25, 164,
            126, 213, 132, 32, 176, 14,
            158, 251, 110, 112, 123, 37,
            1, 242, 142, 201, 205, 12
        }
    )]
    public void ReverseColumnsMixing_Test(byte[] state, byte[] expected)
    {
        _rijndaelMixColumnsService!.ReverseColumnsMixing(state);
        CollectionAssert.AreEqual(expected, state);
    }

    [Test]
    [TestCase(new byte[] { })]
    [TestCase(new byte[] { 42 })]
    [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })]
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
            () => _rijndaelMixColumnsService!.MixColumns(state)
        );
        Assert.Throws<ArgumentException>(
            () => _rijndaelMixColumnsService!.ReverseColumnsMixing(state)
        );
    }
}