using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationResultsVM
{
    BigInteger P { get; }
    BigInteger Q { get; }
}