using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAFactorizationAttackVM
{
    IRSAFactorizationAttackParametersVM Parameters { get; }
    IRSAFactorizationAttackResultsVM Results { get; }

    bool IsInProgress { get; }

    ICommand Attack { get; }
    ICommand Cancel { get; }
}