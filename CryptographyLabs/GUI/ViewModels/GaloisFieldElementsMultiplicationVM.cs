using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories.Abstract;
using Module.Rijndael.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class GaloisFieldElementsMultiplicationVM : IGaloisFieldElementsMultiplicationVM
{
    public string GeneratingElement { get; set; } = string.Empty;
    public string First { get; set; } = string.Empty;
    public string Second { get; set; } = string.Empty;
    public string MultiplicationResult { get; private set; } = string.Empty;

    public string CalculatedGeneratingElements { get; private set; } = string.Empty;

    public ICommand Multiply => _multiply
        ??= new RelayCommand(_ => Multiply_Internal());

    public ICommand CalculateGeneratingElements => _calculateGeneratingElements
        ??= new RelayCommand(_ => CalculateGeneratingElements_Internal());

    private ICommand? _multiply;
    private ICommand? _calculateGeneratingElements;

    private readonly IGaloisFieldConfigurationFactory _galoisFieldConfigurationFactory;
    private readonly IGaloisFieldService _galoisFieldService;

    private readonly Func<IGaloisFieldConfiguration, IGaloisFieldCalculationService>
        _galoisFieldCalculationServiceFactory;

    public GaloisFieldElementsMultiplicationVM(
        IGaloisFieldConfigurationFactory galoisFieldConfigurationFactory,
        IGaloisFieldService galoisFieldService,
        Func<IGaloisFieldConfiguration, IGaloisFieldCalculationService> galoisFieldCalculationServiceFactory)
    {
        _galoisFieldConfigurationFactory = galoisFieldConfigurationFactory;
        _galoisFieldService = galoisFieldService;
        _galoisFieldCalculationServiceFactory = galoisFieldCalculationServiceFactory;
    }

    private void Multiply_Internal()
    {
        if (!StringEx.TryParse(GeneratingElement, out uint generatingElementTemp)
            || generatingElementTemp > ushort.MaxValue) // костыль, т.к. не хочется добавлять еще один метод копипастом
        {
            MessageBox.Show("Could not parse first value.");
            return;
        }

        var generatingElement = (ushort)generatingElementTemp;

        if (!StringEx.TryParse(First, out byte first))
        {
            MessageBox.Show("Could not parse first value.");
            return;
        }

        if (!StringEx.TryParse(Second, out byte second))
        {
            MessageBox.Show("Could not parse second value.");
            return;
        }

        if (!_galoisFieldService.IsGeneratingElement(generatingElement))
        {
            MessageBox.Show("Provided generating element is not a generating element actually.");
            return;
        }

        var galoisFieldConfiguration = _galoisFieldConfigurationFactory.Create(generatingElement);

        var galoisFieldCalculationService = _galoisFieldCalculationServiceFactory(galoisFieldConfiguration);

        var result = galoisFieldCalculationService.Multiply(first, second);
        MultiplicationResult = "0b" + Convert.ToString(result, 2);
    }

    private void CalculateGeneratingElements_Internal()
    {
        CalculatedGeneratingElements = string.Join(
            Environment.NewLine,
            _galoisFieldService.CalculateGeneratingElements()
                .Select((x, i) => $"{i}: 0b{Convert.ToString(x, 2)}")
        );
    }
}