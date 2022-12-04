using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IGaloisFieldElementRepresentationVM
{
    string InputGaloisFieldElement { get; set; }
    string PolynomialRepresentation { get; }

    string InputPolynomial { get; set; }
    string ParsedGaloisFieldElement { get; }

    ICommand RepresentAsPolynomial { get; }
    ICommand ParseToGaloisFieldElement { get; }
}