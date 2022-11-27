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

    private readonly IGaloisFieldRepresentationService _galoisFieldRepresentationService;

    public GaloisFieldElementRepresentationVM(IGaloisFieldRepresentationService galoisFieldRepresentationService)
    {
        _galoisFieldRepresentationService = galoisFieldRepresentationService;
    }

    private void RepresentAsPolynomial_Internal()
    {
        if (!StringEx.TryParse(InputGaloisFieldElement, out byte value))
        {
            MessageBox.Show("Could not parse input value.");
            return;
        }

        PolynomialRepresentation = _galoisFieldRepresentationService.ToStringAsPolynomial(value);
    }

    private void ParseToGaloisFieldElement_Internal()
    {
        if (!_galoisFieldRepresentationService.TryParseAsPolynomial(InputPolynomial, out var value))
        {
            MessageBox.Show("Could not parse input polynomial.");
            return;
        }

        ParsedGaloisFieldElement = "0b" + Convert.ToString(value, 2);
    }
}