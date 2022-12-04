using Module.PermutationNetwork.Exceptions;

namespace Module.PermutationNetwork.Services.Abstract;

public interface IPermutationMasksCalculator
{
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PermutationMasksCalculationException"></exception>
    uint[] Calculate32(byte[] permutation);

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PermutationMasksCalculationException"></exception>
    ulong[] Calculate64(byte[] permutation);
}