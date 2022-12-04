using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAKeyGenerationVM
{
    IRSAKeyGenerationParametersVM Parameters { get; }
    IRSAKeyGenerationResultsVM Results { get; }

    ICommand Generate { get; }
}