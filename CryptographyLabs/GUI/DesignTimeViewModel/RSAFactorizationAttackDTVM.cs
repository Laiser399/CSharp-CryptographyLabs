using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAFactorizationAttackDTVM : IRSAFactorizationAttackVM
{
    public IRSAFactorizationAttackParametersVM Parameters { get; } = new RSAFactorizationAttackParametersDTVM();
    public IRSAFactorizationAttackResultsVM Results { get; } = new RSAFactorizationAttackResultsDTVM();

    public bool IsInProgress => false;

    public ICommand Attack { get; } = new RelayCommand(_ => { });
    public ICommand Cancel { get; } = new RelayCommand(_ => { });
}