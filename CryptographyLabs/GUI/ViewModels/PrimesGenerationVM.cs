using System;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.ViewModels;

public class PrimesGenerationVM : IPrimesGenerationVM
{
    public IPrimesGenerationParametersVM Parameters { get; }
    public IPrimesGenerationResultsVM Results { get; }

    public ICommand Generate => _generate ??= new RelayCommand(_ => Generate_Internal());
    private ICommand? _generate;

    public PrimesGenerationVM(
        IPrimesGenerationParametersVM parametersVM,
        IPrimesGenerationResultsVM resultsVM)
    {
        Parameters = parametersVM;
        Results = resultsVM;
    }

    private void Generate_Internal()
    {
        throw new NotImplementedException();
    }
}