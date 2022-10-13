using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Enums;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAAttackDTVM : IRSAAttackVM
{
    public IReadOnlyCollection<IRSAAttackTypeVM> AttackTypes { get; } = new[]
    {
        new RSAAttackTypeDTVM(RSAAttackType.Factorization, "Factorization attack"),
        new RSAAttackTypeDTVM(RSAAttackType.Wiener, "Wiener attack")
    };

    public IRSAAttackTypeVM SelectedAttackType { get; set; }

    public IRSAAttackParametersVM Parameters { get; } = new RSAAttackParametersDTVM();
    public IRSAAttackResultsVM Results { get; } = new RSAAttackResultsDTVM();

    public bool IsInProgress => false;

    public ICommand Attack { get; } = new RelayCommand(_ => { });
    public ICommand Cancel { get; } = new RelayCommand(_ => { });

    public RSAAttackDTVM()
    {
        SelectedAttackType = AttackTypes.First();
    }
}