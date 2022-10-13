using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAAttackResultsVM
{
    BigInteger PrivateExponent { get; set; }
}