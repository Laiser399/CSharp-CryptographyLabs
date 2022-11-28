using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.Rijndael.Services.Abstract;

namespace CryptographyLabs.GUI.ViewModels;

public class BinaryPolynomialMultiplicationVM : IBinaryPolynomialMultiplicationVM
{
    public string FirstPolynomial { get; set; } = string.Empty;
    public string SecondPolynomial { get; set; } = string.Empty;
    public string MultiplicationResult { get; private set; } = string.Empty;

    public ICommand Multiply => _multiply
        ??= new RelayCommand(_ => Multiply_Internal());

    private ICommand? _multiply;

    private readonly IBinaryPolynomialRepresentationService _binaryPolynomialRepresentationService;
    private readonly IBinaryPolynomialsCalculationService _binaryPolynomialsCalculationService;

    public BinaryPolynomialMultiplicationVM(
        IBinaryPolynomialRepresentationService binaryPolynomialRepresentationService,
        IBinaryPolynomialsCalculationService binaryPolynomialsCalculationService)
    {
        _binaryPolynomialRepresentationService = binaryPolynomialRepresentationService;
        _binaryPolynomialsCalculationService = binaryPolynomialsCalculationService;
    }

    private void Multiply_Internal()
    {
        if (!_binaryPolynomialRepresentationService.TryParseAsPolynomial(FirstPolynomial, out var first))
        {
            MessageBox.Show("Could not parse first polynomial.");
            return;
        }

        if (!_binaryPolynomialRepresentationService.TryParseAsPolynomial(SecondPolynomial, out var second))
        {
            MessageBox.Show("Could not parse second polynomial.");
            return;
        }

        var result = _binaryPolynomialsCalculationService.Multiply(first, second);

        MultiplicationResult = _binaryPolynomialRepresentationService.ToStringAsPolynomial(result);
    }
}