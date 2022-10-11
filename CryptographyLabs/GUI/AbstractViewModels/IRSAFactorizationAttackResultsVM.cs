using System;
using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAFactorizationAttackResultsVM
{
    BigInteger PrivateExponent { get; set; }

    [Obsolete] BigInteger FirstFactor { get; set; }
    [Obsolete] BigInteger SecondFactor { get; set; }
}