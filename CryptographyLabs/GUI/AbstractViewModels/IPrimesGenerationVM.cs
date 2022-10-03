using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationVM
{
    IPrimesGenerationParametersVM Parameters { get; }
    IPrimesGenerationResultsVM Results { get; }

    ICommand Generate { get; }
}