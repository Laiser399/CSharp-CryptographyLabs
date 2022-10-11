using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAFactorizationAttackResultsDTVM : IRSAFactorizationAttackResultsVM
{
    public BigInteger PrivateExponent { get; set; } = BigInteger.Parse("304958673409563");

    public BigInteger FirstFactor { get; set; } = BigInteger.Parse("304958673409568");
    public BigInteger SecondFactor { get; set; } = BigInteger.Parse("9485765");
}