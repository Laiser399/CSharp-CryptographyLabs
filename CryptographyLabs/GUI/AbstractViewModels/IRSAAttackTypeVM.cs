using Module.RSA.Enums;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAAttackTypeVM
{
    RSAAttackType Type { get; }
    string Name { get; }
}