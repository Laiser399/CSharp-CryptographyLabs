using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IBinaryPolynomialMultiplicationVM
{
    string FirstPolynomial { get; set; }
    string SecondPolynomial { get; set; }
    string MultiplicationResult { get; }

    ICommand Multiply { get; }
}