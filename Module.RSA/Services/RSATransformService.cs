using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSATransformService : IRSATransformService
{
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;

    public RSATransformService(IBigIntegerCalculationService bigIntegerCalculationService)
    {
        _bigIntegerCalculationService = bigIntegerCalculationService;
    }

    public byte[] Encrypt(byte[] data, IRSAKey key)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Array is empty.", nameof(data));
        }

        var nByteCount = key.Modulus.GetByteCount(true);
        var inputBlockSize = nByteCount - 1;

        return Transform(data, key, inputBlockSize, nByteCount);
    }

    public byte[] Decrypt(byte[] data, IRSAKey key)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Array is empty.", nameof(data));
        }

        var nByteCount = key.Modulus.GetByteCount(true);
        var outputBlockSize = nByteCount - 1;

        return Transform(data, key, nByteCount, outputBlockSize);
    }

    private byte[] Transform(byte[] data, IRSAKey key, int inputBlockSize, int outputBlockSize)
    {
        var blockCount = (int)Math.Ceiling((double)data.Length / inputBlockSize);

        var block = new byte[inputBlockSize];

        var result = new List<byte>();
        for (var i = 0; i < blockCount; i++)
        {
            var bytesCountToCopy = i < blockCount - 1
                ? inputBlockSize
                : data.Length - i * inputBlockSize;

            Array.Copy(data, i * inputBlockSize, block, 0, bytesCountToCopy);
            Array.Fill(block, (byte)0, bytesCountToCopy, inputBlockSize - bytesCountToCopy);

            var transformedBlock = TransformBlock(block, key);

            result.AddRange(transformedBlock);
            if (i < blockCount - 1)
            {
                result.AddRange(Enumerable.Repeat((byte)0, outputBlockSize - transformedBlock.Length));
            }
        }

        return result.ToArray();
    }

    private byte[] TransformBlock(byte[] block, IRSAKey key)
    {
        var value = new BigInteger(block, true);

        var transformedValue = _bigIntegerCalculationService.BinPowMod(value, key.Exponent, key.Modulus);
        return transformedValue.ToByteArray(true);
    }
}