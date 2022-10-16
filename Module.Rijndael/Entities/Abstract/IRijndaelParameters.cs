namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelParameters
{
    int BlockSize { get; }
    int RoundCount { get; }


    Span<byte> InitialKey { get; }

    Span<byte> GetRoundKey(int round);
}