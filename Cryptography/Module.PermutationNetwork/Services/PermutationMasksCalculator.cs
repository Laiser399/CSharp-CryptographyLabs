using Module.PermutationNetwork.Exceptions;
using Module.PermutationNetwork.Networks;
using Module.PermutationNetwork.Services.Abstract;

namespace Module.PermutationNetwork.Services;

public class PermutationMasksCalculator : IPermutationMasksCalculator
{
    public uint[] Calculate32(byte[] permutation)
    {
        if (permutation.Length != 32)
        {
            throw new ArgumentException("Wrong length of permutation.");
        }

        if (permutation.Distinct().Count() != 32)
        {
            throw new ArgumentException("There is repeats in permutation.");
        }

        if (permutation.Any(x => x >= 32))
        {
            throw new ArgumentException("Found value in permutation greater or equal 64.");
        }

        var inversedPermutation = new byte[32];
        for (var i = 0; i < 32; ++i)
        {
            inversedPermutation[permutation[i]] = (byte)i;
        }

        var network = new PermutationNetwork32(inversedPermutation);

        if (network.Masks.Count != 9)
        {
            throw new PermutationMasksCalculationException("Something went wrong - masks count is not equal 11.");
        }

        return network.Masks.ToArray();
    }

    public ulong[] Calculate64(byte[] permutation)
    {
        if (permutation.Length != 64)
        {
            throw new ArgumentException("Wrong length of permutation.");
        }

        if (permutation.Distinct().Count() != 64)
        {
            throw new ArgumentException("There is repeats in permutation.");
        }

        if (permutation.Any(x => x >= 64))
        {
            throw new ArgumentException("Found value in permutation greater or equal 64.");
        }

        var inversedPermutation = new byte[64];
        for (var i = 0; i < 64; ++i)
        {
            inversedPermutation[permutation[i]] = (byte)i;
        }

        var network = new PermutationNetwork64(inversedPermutation);

        if (network.Masks.Count != 11)
        {
            throw new PermutationMasksCalculationException("Something went wrong - masks count is not equal 11.");
        }

        return network.Masks.ToArray();
    }
}