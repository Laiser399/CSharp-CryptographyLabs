using System;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.Rijndael.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class GaloisFieldElementInversionVM : IGaloisFieldElementInversionVM
{
    public string InputValue { get; set; } = string.Empty;
    public string InversedValue { get; private set; } = string.Empty;

    public ICommand Inverse => _inverse ??= new RelayCommand(_ => Inverse_Internal());
    private ICommand? _inverse;

    private readonly IGaloisFieldCalculationService _galoisFieldCalculationService;

    public GaloisFieldElementInversionVM(IGaloisFieldCalculationService galoisFieldCalculationService)
    {
        _galoisFieldCalculationService = galoisFieldCalculationService;
    }

    private void Inverse_Internal()
    {
        if (!StringEx.TryParse(InputValue, out byte value))
        {
            MessageBox.Show("Could not parse input value.");
            return;
        }

        var inversed = _galoisFieldCalculationService.Inverse(value);
        InversedValue = $"0b{Convert.ToString(inversed, 2)}; {inversed}.";
    }
}