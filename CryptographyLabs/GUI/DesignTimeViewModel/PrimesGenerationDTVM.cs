using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class PrimesGenerationDTVM : IPrimesGenerationVM
{
    public IPrimesGenerationParametersVM Parameters { get; } = new PrimesGenerationParametersDTVM();
    public IPrimesGenerationResultsVM Results { get; } = new PrimesGenerationResultsDTVM();

    public ICommand Generate { get; } = new RelayCommand(_ => { });
}