using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAFactorizationAttackResultsVM
{
    BigInteger FirstFactor { get; set; }
    BigInteger SecondFactor { get; set; }
}