using Module.Rijndael.Entities.Abstract;

namespace Module.Rijndael.UnitTests.Entities;

public class GaloisFieldConfiguration : IGaloisFieldConfiguration
{
    public ushort IrreduciblePolynomial { get; }

    public GaloisFieldConfiguration(ushort irreduciblePolynomial)
    {
        IrreduciblePolynomial = irreduciblePolynomial;
    }
}