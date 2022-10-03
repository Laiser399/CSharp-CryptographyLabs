using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class PrimesGenerationResultsVM : IPrimesGenerationResultsVM
{
    public BigInteger P { get; set; } = 0;
    public BigInteger Q { get; set; } = 0;
}