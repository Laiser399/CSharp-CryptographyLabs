using System.Security.Cryptography;
using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class OfbDecryptTransform : ICryptoTransform
{
    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public OfbDecryptTransform(IBlockCryptoTransform blockCryptoTransform, byte[] initialVector)
    {
    }

    public int TransformBlock(
        byte[] inputBuffer,
        int inputOffset,
        int inputCount,
        byte[] outputBuffer,
        int outputOffset)
    {
        throw new NotImplementedException();
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}