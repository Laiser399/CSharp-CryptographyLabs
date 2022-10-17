using Module.Rijndael.Entities.Abstract;

namespace Module.Rijndael.Factories.Abstract;

public interface IGaloisFieldConfigurationFactory
{
    IGaloisFieldConfiguration Default { get; }

    /// <exception cref="ArgumentException">Provided polynomial is not generating for Galois field.</exception>
    IGaloisFieldConfiguration Create(ushort irreduciblePolynomial);
}