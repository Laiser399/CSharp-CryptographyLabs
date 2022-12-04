using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class GaloisFieldElementRepresentationDTVM : IGaloisFieldElementRepresentationVM
{
    public string InputGaloisFieldElement { get; set; } = "0b1011";
    public string PolynomialRepresentation => "x^3 + x + 1";

    public string InputPolynomial { get; set; } = "x^4 + x^2 + 1";
    public string ParsedGaloisFieldElement => "0b10101";

    public ICommand RepresentAsPolynomial => RelayCommand.Empty;
    public ICommand ParseToGaloisFieldElement => RelayCommand.Empty;
}