using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class BinaryPolynomialMultiplicationDTVM : IBinaryPolynomialMultiplicationVM
{
    public string FirstPolynomial { get; set; } = "x^7 + x + 1";
    public string SecondPolynomial { get; set; } = "x^31 + x^3";
    public string MultiplicationResult => "x^38 + x^32 + x^31 + x^10 + x^4 + x^3";

    public ICommand Multiply => RelayCommand.Empty;
}