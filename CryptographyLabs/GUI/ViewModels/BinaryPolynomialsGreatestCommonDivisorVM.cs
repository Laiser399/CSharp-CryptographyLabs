using System;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.Rijndael.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class BinaryPolynomialsGreatestCommonDivisorVM : IBinaryPolynomialsGreatestCommonDivisorVM
{
    public string A { get; set; } = string.Empty;
    public string B { get; set; } = string.Empty;

    public string X { get; private set; } = string.Empty;
    public string Y { get; private set; } = string.Empty;
    public string GreatestCommonDivisor { get; private set; } = string.Empty;

    public ICommand Calculate => _calculate
        ??= new RelayCommand(_ => Calculate_Internal());

    private ICommand? _calculate;

    private readonly IBinaryPolynomialsCalculationService _binaryPolynomialsCalculationService;

    public BinaryPolynomialsGreatestCommonDivisorVM(
        IBinaryPolynomialsCalculationService binaryPolynomialsCalculationService)
    {
        _binaryPolynomialsCalculationService = binaryPolynomialsCalculationService;
    }

    private void Calculate_Internal()
    {
        if (!StringEx.TryParse(A, out uint a))
        {
            MessageBox.Show("Could not parse \"a\".");
            return;
        }

        if (!StringEx.TryParse(B, out uint b))
        {
            MessageBox.Show("Could not parse \"b\".");
            return;
        }

        var gcd = _binaryPolynomialsCalculationService.GreatestCommonDivisor(a, b, out var x, out var y);

        X = $"0b{Convert.ToString(x, 2)}; {x}.";
        Y = $"0b{Convert.ToString(y, 2)}; {y}.";
        GreatestCommonDivisor = $"0b{Convert.ToString(gcd, 2)}; {gcd}.";
    }
}