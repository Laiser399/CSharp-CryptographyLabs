using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Enums;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAAttackTypeDTVM : IRSAAttackTypeVM
{
    public RSAAttackType Type { get; }
    public string Name { get; }

    public RSAAttackTypeDTVM(RSAAttackType type, string name)
    {
        Type = type;
        Name = name;
    }
}