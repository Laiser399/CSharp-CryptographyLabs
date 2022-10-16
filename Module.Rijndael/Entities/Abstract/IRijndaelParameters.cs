namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelParameters
{
    int BlockSize { get; }
    int RoundCount { get; }


    ReadOnlySpan<byte> InitialKey { get; }

    ReadOnlySpan<byte> GetRoundKey(int round);
}