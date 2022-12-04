using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class BinaryPolynomialsGreatestCommonDivisorDTVM : IBinaryPolynomialsGreatestCommonDivisorVM
{
    public string A { get; set; } = "0b101";
    public string B { get; set; } = "0b1001";

    public string X => "0b10";
    public string Y => "0b1";
    public string GreatestCommonDivisor => "0b11";

    public ICommand Calculate => RelayCommand.Empty;
}