using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAAttackDTVM : IRSAAttackVM
{
    public IRSAAttackParametersVM Parameters { get; } = new RSAAttackParametersDTVM();
    public IRSAAttackResultsVM Results { get; } = new RSAAttackResultsDTVM();

    public bool IsInProgress => false;

    public ICommand Attack { get; } = new RelayCommand(_ => { });
    public ICommand Cancel { get; } = new RelayCommand(_ => { });
}