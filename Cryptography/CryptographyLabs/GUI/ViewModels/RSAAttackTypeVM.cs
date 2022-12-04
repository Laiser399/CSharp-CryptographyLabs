using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Enums;

namespace CryptographyLabs.GUI.ViewModels;

public class RSAAttackTypeVM : IRSAAttackTypeVM
{
    public RSAAttackType Type { get; }
    public string Name { get; }

    public RSAAttackTypeVM(RSAAttackType type, string name)
    {
        Type = type;
        Name = name;
    }
}