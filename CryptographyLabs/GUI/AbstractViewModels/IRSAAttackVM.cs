using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAAttackVM
{
    IRSAAttackParametersVM Parameters { get; }
    IRSAAttackResultsVM Results { get; }

    bool IsInProgress { get; }

    ICommand Attack { get; }
    ICommand Cancel { get; }
}