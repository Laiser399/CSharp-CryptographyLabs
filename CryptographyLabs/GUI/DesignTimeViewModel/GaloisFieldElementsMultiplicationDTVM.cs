using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class GaloisFieldElementsMultiplicationDTVM : IGaloisFieldElementsMultiplicationVM
{
    public string GeneratingElement { get; set; } = "0b100011011";
    public string First { get; set; } = "0b1001101";
    public string Second { get; set; } = "0b1100110";
    public string MultiplicationResult => "0b10000";

    public string CalculatedGeneratingElements => "0: 0b100011011\n1: 0b100011101";

    public ICommand Multiply => RelayCommand.Empty;
    public ICommand CalculateGeneratingElements => RelayCommand.Empty;
}