using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSATransformationVM
{
    IRSATransformationParametersVM Parameters { get; }

    bool IsInProgress { get; }
    double Progress { get; }

    ICommand Transform { get; }
}