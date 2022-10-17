using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Factories;

public class GaloisFieldConfigurationFactory : IGaloisFieldConfigurationFactory
{
    public static readonly IGaloisFieldConfiguration DefaultConfiguration = new GaloisFieldConfiguration(0b100011011);

    private readonly IGaloisFieldService _galoisFieldService;

    public GaloisFieldConfigurationFactory(IGaloisFieldService galoisFieldService)
    {
        _galoisFieldService = galoisFieldService;
    }

    public IGaloisFieldConfiguration Create(ushort irreduciblePolynomial)
    {
        if (!_galoisFieldService.IsGeneratingElement(irreduciblePolynomial))
        {
            throw new ArgumentException(
                "Provided polynomial could not be generating polynomial for Galois field.",
                nameof(irreduciblePolynomial)
            );
        }

        return new GaloisFieldConfiguration(irreduciblePolynomial);
    }

    private class GaloisFieldConfiguration : IGaloisFieldConfiguration
    {
        public ushort IrreduciblePolynomial { get; }

        public GaloisFieldConfiguration(ushort irreduciblePolynomial)
        {
            IrreduciblePolynomial = irreduciblePolynomial;
        }
    }
}