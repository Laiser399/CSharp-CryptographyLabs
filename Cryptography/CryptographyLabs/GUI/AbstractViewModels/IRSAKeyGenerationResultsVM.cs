using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAKeyGenerationResultsVM
{
    BigInteger PublicExponent { get; set; }
    BigInteger PrivateExponent { get; set; }
    BigInteger Modulus { get; set; }
}