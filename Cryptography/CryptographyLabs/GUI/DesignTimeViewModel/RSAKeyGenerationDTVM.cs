using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAKeyGenerationDTVM : IRSAKeyGenerationVM
{
    public IRSAKeyGenerationParametersVM Parameters { get; } = new RSAKeyGenerationParametersDTVM();
    public IRSAKeyGenerationResultsVM Results { get; } = new RSAKeyGenerationResultsDTVM();

    public ICommand Generate { get; } = new RelayCommand(_ => { });
}