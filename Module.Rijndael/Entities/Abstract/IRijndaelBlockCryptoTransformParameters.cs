namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelBlockCryptoTransformParameters
{
    int BlockSize { get; }
    int RoundCount { get; }


    ReadOnlySpan<byte> InitialKey { get; }

    ReadOnlySpan<byte> GetRoundKey(int round);
}