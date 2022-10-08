using System;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.ViewModels;

public class RSAFactorizationAttackVM : IRSAFactorizationAttackVM
{
    public IRSAFactorizationAttackParametersVM Parameters { get; }
    public IRSAFactorizationAttackResultsVM Results { get; }

    public bool IsInProgress { get; private set; }

    public ICommand Attack => _attack ??= new RelayCommand(_ => Attack_Internal());
    public ICommand Cancel => _cancel ??= new RelayCommand(_ => Cancel_Internal());

    private ICommand? _attack;
    private ICommand? _cancel;

    public RSAFactorizationAttackVM(
        IRSAFactorizationAttackParametersVM parameters,
        IRSAFactorizationAttackResultsVM results)
    {
        Parameters = parameters;
        Results = results;
    }

    private void Attack_Internal()
    {
        throw new NotImplementedException();
    }

    private void Cancel_Internal()
    {
        throw new NotImplementedException();
    }
}