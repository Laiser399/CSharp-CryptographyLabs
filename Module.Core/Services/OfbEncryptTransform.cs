using System.Security.Cryptography;
using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class OfbEncryptTransform : ICryptoTransform
{
    public bool CanReuseTransform => false;
    public bool CanTransformMultipleBlocks => false;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;
    private readonly IXorService _xorService;

    private readonly byte[] _encryptionVector;

    public OfbEncryptTransform(IBlockCryptoTransform blockCryptoTransform, byte[] initialVector, IXorService xorService)
    {
        if (blockCryptoTransform.InputBlockSize != blockCryptoTransform.OutputBlockSize)
        {
            throw new ArgumentException(
                "Transform with different input and output block sizes is not supported",
                nameof(blockCryptoTransform)
            );
        }

        if (blockCryptoTransform.InputBlockSize != initialVector.Length)
        {
            throw new ArgumentException(
                "Initial vector size is not equal transform block size.",
                nameof(initialVector)
            );
        }

        _blockCryptoTransform = blockCryptoTransform;
        _xorService = xorService;

        _encryptionVector = new byte[InputBlockSize];
        initialVector.CopyTo(_encryptionVector, 0);
    }

    public int TransformBlock(
        byte[] inputBuffer,
        int inputOffset,
        int inputCount,
        byte[] outputBuffer,
        int outputOffset)
    {
        if (inputCount != InputBlockSize)
        {
            return 0;
        }

        var input = new Span<byte>(inputBuffer, inputOffset, InputBlockSize);
        var output = new Span<byte>(outputBuffer, outputOffset, InputBlockSize);

        _blockCryptoTransform.Transform(_encryptionVector, output);

        output.CopyTo(_encryptionVector);

        _xorService.Xor(output, input, output);

        return InputBlockSize;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        var input = new Span<byte>(inputBuffer, inputOffset, InputBlockSize);
        var output = new byte[InputBlockSize];

        input[^1] = (byte)inputCount;

        _blockCryptoTransform.Transform(_encryptionVector, output);

        _xorService.Xor(output, input, output);

        return output;
    }

    public void Dispose()
    {
    }
}