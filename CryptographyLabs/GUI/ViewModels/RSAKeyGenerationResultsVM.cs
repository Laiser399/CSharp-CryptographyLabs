using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAKeyGenerationResultsVM : IRSAKeyGenerationResultsVM
{
    public BigInteger PublicExponent { get; set; }
    public BigInteger PrivateExponent { get; set; }
    public BigInteger Modulus { get; set; }
}