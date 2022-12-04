using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationVM
{
    IPrimesGenerationParametersVM Parameters { get; }
    IPrimesGenerationResultsVM Results { get; }

    bool IsInProgress { get; }
    string GenerationTextAnimation { get; }
    ICommand Generate { get; }
}