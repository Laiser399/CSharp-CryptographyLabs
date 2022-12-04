using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAKeyGenerationResultsDTVM : IRSAKeyGenerationResultsVM
{
    public BigInteger PublicExponent { get; set; } = BigInteger.Parse("65537");
    public BigInteger PrivateExponent { get; set; } = BigInteger.Parse("2304985723049581");
    public BigInteger Modulus { get; set; } = BigInteger.Parse("2093487562309485732409587");
}