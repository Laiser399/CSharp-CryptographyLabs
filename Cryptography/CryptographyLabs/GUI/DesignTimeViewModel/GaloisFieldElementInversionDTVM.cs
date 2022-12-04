using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class GaloisFieldElementInversionDTVM : IGaloisFieldElementInversionVM
{
    public string InputValue { get; set; } = "0";
    public string InversedValue => "0";

    public ICommand Inverse => RelayCommand.Empty;
}