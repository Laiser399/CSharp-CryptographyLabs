using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IGaloisFieldElementsMultiplicationVM
{
    string GeneratingElement { get; set; }
    string First { get; set; }
    string Second { get; set; }
    string MultiplicationResult { get; }

    string CalculatedGeneratingElements { get; }

    ICommand Multiply { get; }
    ICommand CalculateGeneratingElements { get; }
}