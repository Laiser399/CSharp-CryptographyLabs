using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Extensions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class PrimesPairGenerator : IPrimesPairGenerator
{
    private readonly IRandomProvider _randomProvider;
    private readonly IPrimesPairGeneratorParameters _parameters;
    private readonly IPrimalityTester _primalityTester;

    private readonly BigInteger _maxValue;

    public PrimesPairGenerator(
        IRandomProvider randomProvider,
        IPrimesPairGeneratorParameters parameters,
        IPrimalityTester primalityTester)
    {
        _randomProvider = randomProvider;
        _parameters = parameters;
        _primalityTester = primalityTester;

        _maxValue = GetMaxValue();
    }

    private BigInteger GetMaxValue()
    {
        var bytes = new byte[_parameters.ByteCount + 1];

        bytes[^1] = 0b00000000;
        for (var i = 0; i < _parameters.ByteCount; i++)
        {
            bytes[i] = 0b11111111;
        }

        return new BigInteger(bytes);
    }

    // todo cancellation token везде где есть Async
    public Task<(BigInteger p, BigInteger q)> GenerateAsync()
    {
        return Task.Run(() =>
        {
            Generate(out var p, out var q);
            return (p, q);
        });
    }

    public void Generate(out BigInteger p, out BigInteger q)
    {
        p = GenerateP();
        q = GenerateQ(p);
    }

    private BigInteger GenerateP()
    {
        while (true)
        {
            var initialP = GetInitialP();
            if (TryGeneratePByAdding(initialP, out var p))
            {
                return p;
            }
        }
    }

    private bool TryGeneratePByAdding(BigInteger initialP, out BigInteger p)
    {
        p = initialP;

        for (var i = 0; i < _parameters.AddingTriesCount; i++)
        {
            if (_primalityTester.TestIsPrime(p))
            {
                return true;
            }

            p += 2;
            if (p > _maxValue)
            {
                return false;
            }
        }

        return false;
    }

    private BigInteger GetInitialP()
    {
        var pBytes = new byte[_parameters.ByteCount + 1];
        _randomProvider.Random.NextBytes(pBytes);
        PrepareBytes(pBytes);
        return new BigInteger(pBytes);
    }

    private BigInteger GenerateQ(BigInteger p)
    {
        while (true)
        {
            var initialQ = GetInitialQ(p);
            if (TryGenerateQByAdding(p, initialQ, out var q))
            {
                return q;
            }
        }
    }

    private BigInteger GetInitialQ(BigInteger p)
    {
        var pSecondSignificantByte = p.ToByteArray()[^2];

        var qBytes = new byte[_parameters.ByteCount + 1];
        _randomProvider.Random.NextBytes(qBytes);
        PrepareBytes(qBytes);
        qBytes[^2] ^= (byte)(~(pSecondSignificantByte ^ qBytes[^2]) & 0b01000000);

        return new BigInteger(qBytes);
    }

    private bool TryGenerateQByAdding(BigInteger p, BigInteger initialQ, out BigInteger q)
    {
        q = initialQ;

        for (var i = 0; i < _parameters.AddingTriesCount; i++)
        {
            if (!HasEnoughDifference(p, q))
            {
                return false;
            }

            if (p != q && _primalityTester.TestIsPrime(q))
            {
                return true;
            }

            q += 2;
            if (q > _maxValue)
            {
                return false;
            }
        }

        return false;
    }

    private bool HasEnoughDifference(BigInteger p, BigInteger q)
    {
        if (p > q)
        {
            (p, q) = (q, p);
        }

        var diff = q - p;
        var bitCount = diff.GetBitCount();

        return bitCount >= _parameters.PQDifferenceMinBitCount;
    }

    private static void PrepareBytes(IList<byte> bytes)
    {
        bytes[^1] = 0b00000000;
        bytes[^2] |= 0b10000000;
        bytes[0] |= 0b00000001;
    }
}