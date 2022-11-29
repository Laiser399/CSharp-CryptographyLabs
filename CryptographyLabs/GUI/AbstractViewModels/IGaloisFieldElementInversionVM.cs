using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IGaloisFieldElementInversionVM
{
    string InputValue { get; set; }
    string InversedValue { get; }

    ICommand Inverse { get; }
}