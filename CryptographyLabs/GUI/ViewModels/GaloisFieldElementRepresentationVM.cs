using System;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.Rijndael.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class GaloisFieldElementRepresentationVM : IGaloisFieldElementRepresentationVM
{
    public string InputGaloisFieldElement { get; set; } = string.Empty;
    public string PolynomialRepresentation { get; private set; } = string.Empty;

    public string InputPolynomial { get; set; } = string.Empty;
    public string ParsedGaloisFieldElement { get; private set; } = string.Empty;

    public ICommand RepresentAsPolynomial => _representAsPolynomial
        ??= new RelayCommand(_ => RepresentAsPolynomial_Internal());

    public ICommand ParseToGaloisFieldElement => _parseToGaloisFieldElement
        ??= new RelayCommand(_ => ParseToGaloisFieldElement_Internal());

    private ICommand? _representAsPolynomial;
    private ICommand? _parseToGaloisFieldElement;

    private readonly IBinaryPolynomialRepresentationService _binaryPolynomialRepresentationService;

    public GaloisFieldElementRepresentationVM(IBinaryPolynomialRepresentationService binaryPolynomialRepresentationService)
    {
        _binaryPolynomialRepresentationService = binaryPolynomialRepresentationService;
    }

    private void RepresentAsPolynomial_Internal()
    {
        if (!StringEx.TryParse(InputGaloisFieldElement, out byte value))
        {
            MessageBox.Show("Could not parse input value.");
            return;
        }

        PolynomialRepresentation = _binaryPolynomialRepresentationService.ToString(value);
    }

    private void ParseToGaloisFieldElement_Internal()
    {
        if (!_binaryPolynomialRepresentationService.TryParse(InputPolynomial, out byte value))
        {
            MessageBox.Show("Could not parse input polynomial.");
            return;
        }

        ParsedGaloisFieldElement = "0b" + Convert.ToString(value, 2);
    }
}