using System.Collections.Generic;
using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAAttackVM
{
    IReadOnlyCollection<IRSAAttackTypeVM> AttackTypes { get; }
    IRSAAttackTypeVM SelectedAttackType { get; set; }

    IRSAAttackParametersVM Parameters { get; }
    IRSAAttackResultsVM Results { get; }

    bool IsInProgress { get; }

    ICommand Attack { get; }
    ICommand Cancel { get; }
}