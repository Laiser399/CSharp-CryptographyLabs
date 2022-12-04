using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAAttackResultsVM : IRSAAttackResultsVM
{
    public BigInteger PrivateExponent { get; set; }
}