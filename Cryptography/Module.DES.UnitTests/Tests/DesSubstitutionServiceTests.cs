using Module.DES.Services;
using Module.DES.Services.Abstract;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class DesSubstitutionServiceTests
{
    private IDesSubstitutionService? _desSubstitutionService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _desSubstitutionService = new DesSubstitutionService();
    }

    [Test]
    [TestCase(
        0b11100101_00001100_01111001_01011011_00000110_10110110ul,
        ExpectedResult = 0b00110011_10111010_01011011_00000111u
    )]
    public uint Test(ulong value48)
    {
        return _desSubstitutionService!.Substitute(value48);
    }
}