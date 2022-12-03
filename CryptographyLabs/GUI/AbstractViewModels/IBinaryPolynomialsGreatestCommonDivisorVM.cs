using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IBinaryPolynomialsGreatestCommonDivisorVM
{
    string A { get; set; }
    string B { get; set; }

    string X { get; }
    string Y { get; }
    string GreatestCommonDivisor { get; }

    ICommand Calculate { get; }
}