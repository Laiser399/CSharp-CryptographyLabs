using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class PrimesGenerationResultsDTVM : IPrimesGenerationResultsVM
{
    public BigInteger P => 3;
    public BigInteger Q { get; } = BigInteger.Parse("29083476023984576234756234985734501781265987659376510");
}