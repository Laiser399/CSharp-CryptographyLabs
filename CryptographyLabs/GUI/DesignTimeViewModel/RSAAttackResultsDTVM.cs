using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAAttackResultsDTVM : IRSAAttackResultsVM
{
    public BigInteger PrivateExponent { get; set; } = BigInteger.Parse("304958673409563");
}