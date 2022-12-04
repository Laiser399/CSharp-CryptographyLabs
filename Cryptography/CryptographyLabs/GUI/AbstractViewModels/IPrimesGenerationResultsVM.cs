using System.Numerics;
using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationResultsVM
{
    BigInteger P { get; set; }
    BigInteger Q { get; set; }

    ICommand CopyPToClipboard { get; }
    ICommand CopyQToClipboard { get; }
}