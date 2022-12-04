using Module.Rijndael.Entities.Abstract;

namespace Module.Rijndael.Factories.Abstract;

public interface IGaloisFieldConfigurationFactory
{
    /// <exception cref="ArgumentException">Provided polynomial is not generating for Galois field.</exception>
    IGaloisFieldConfiguration Create(ushort irreduciblePolynomial);
}