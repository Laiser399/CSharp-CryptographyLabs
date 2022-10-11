using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSATransformService : IRSATransformService
{
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;

    public RSATransformService(IBigIntegerCalculationService bigIntegerCalculationService)
    {
        _bigIntegerCalculationService = bigIntegerCalculationService;
    }

    public Task<byte[]> EncryptAsync(
        byte[] data,
        IRSAKey key,
        CancellationToken? cancellationToken,
        Action<double>? progressCallback)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Array is empty.", nameof(data));
        }

        var nByteCount = key.Modulus.GetByteCount(true);
        var inputBlockSize = nByteCount - 1;

        return TransformAsync(data, key, inputBlockSize, nByteCount, cancellationToken, progressCallback);
    }

    public Task<byte[]> DecryptAsync(
        byte[] data,
        IRSAKey key,
        CancellationToken? cancellationToken,
        Action<double>? progressCallback)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Array is empty.", nameof(data));
        }

        var nByteCount = key.Modulus.GetByteCount(true);
        var outputBlockSize = nByteCount - 1;

        return TransformAsync(data, key, nByteCount, outputBlockSize, cancellationToken, progressCallback);
    }

    private async Task<byte[]> TransformAsync(
        byte[] data,
        IRSAKey key,
        int inputBlockSize,
        int outputBlockSize,
        CancellationToken? cancellationToken,
        Action<double>? progressCallback)
    {
        var blockCount = (int)Math.Ceiling((double)data.Length / inputBlockSize);

        var block = new byte[inputBlockSize];

        var result = new List<byte>();
        for (var i = 0; i < blockCount; i++)
        {
            cancellationToken?.ThrowIfCancellationRequested();

            progressCallback?.Invoke((double)i / blockCount);

            var bytesCountToCopy = i < blockCount - 1
                ? inputBlockSize
                : data.Length - i * inputBlockSize;

            Array.Copy(data, i * inputBlockSize, block, 0, bytesCountToCopy);
            Array.Fill(block, (byte)0, bytesCountToCopy, inputBlockSize - bytesCountToCopy);

            var transformedBlock = await TransformBlockAsync(block, key, cancellationToken);

            if (transformedBlock.Length > outputBlockSize)
            {
                throw new CryptoTransformException("Invalid state of input data.");
            }

            result.AddRange(transformedBlock);
            if (i < blockCount - 1)
            {
                result.AddRange(Enumerable.Repeat((byte)0, outputBlockSize - transformedBlock.Length));
            }
        }

        progressCallback?.Invoke(1);

        return result.ToArray();
    }

    private Task<byte[]> TransformBlockAsync(byte[] block, IRSAKey key, CancellationToken? cancellationToken)
    {
        return Task.Run(
            () =>
            {
                var value = new BigInteger(block, true);

                var transformedValue = _bigIntegerCalculationService.BinPowMod(value, key.Exponent, key.Modulus);
                return transformedValue.ToByteArray(true);
            },
            cancellationToken ?? CancellationToken.None
        );
    }
}