using Module.Rijndael.Entities.Abstract;

namespace Module.Rijndael.UnitTests.Entities;

public class GaloisFieldConfigurationForTests : IGaloisFieldConfiguration
{
    public ushort IrreduciblePolynomial { get; }

    public GaloisFieldConfigurationForTests(ushort irreduciblePolynomial)
    {
        IrreduciblePolynomial = irreduciblePolynomial;
    }
}