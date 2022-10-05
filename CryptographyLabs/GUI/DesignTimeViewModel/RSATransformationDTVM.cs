using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSATransformationDTVM : IRSATransformationVM
{
    public IRSATransformationParametersVM Parameters { get; } = new RSATransformationParametersDTVM();

    public bool IsInProgress => true;
    public double Progress => 0.4;

    public ICommand Transform { get; } = new RelayCommand(_ => { });
}