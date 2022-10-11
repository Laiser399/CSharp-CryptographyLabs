using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAFactorizationAttackResultsVM
{
    BigInteger PrivateExponent { get; set; }
}