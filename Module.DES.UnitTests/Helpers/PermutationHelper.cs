namespace Module.DES.UnitTests.Helpers;

public static class PermutationHelper
{
    public static uint PermuteWithTable(uint value, IReadOnlyList<byte> permutationTable)
    {
        var result = 0u;
        for (var i = 0; i < permutationTable.Count; i++)
        {
            result |= ((value >> permutationTable[i]) & 1) << i;
        }

        return result;
    }

    public static ulong PermuteWithTable(ulong value, IReadOnlyList<byte> permutationTable)
    {
        var result = 0ul;
        for (var i = 0; i < permutationTable.Count; i++)
        {
            result |= ((value >> permutationTable[i]) & 1) << i;
        }

        return result;
    }
}