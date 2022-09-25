using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Extensions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class PrimesPairGenerator : IPrimesPairGenerator
{
    private readonly IPrimesPairGeneratorParameters _parameters;
    private readonly IPrimalityTester _primalityTester;

    private readonly Random _random;

    private readonly BigInteger _maxValue;

    public PrimesPairGenerator(
        IPrimesPairGeneratorParameters parameters,
        IPrimalityTester primalityTester)
    {
        _parameters = parameters;
        _primalityTester = primalityTester;

        _random = new Random();
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

    public void Generate(out BigInteger p, out BigInteger q)
    {
        while (true)
        {
            p = GenerateP();
            if (TryGenerateQ(p, out q))
            {
                return;
            }
        }
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

        for (var i = 0; i < _parameters.StepTriesCount; i++)
        {
            if (_primalityTester.TestIsPrime(p, _parameters.PrimalityProbability))
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
        _random.NextBytes(pBytes);
        PrepareBytes(pBytes);
        return new BigInteger(pBytes);
    }

    private bool TryGenerateQ(BigInteger p, out BigInteger q)
    {
        for (var i = 0; i < _parameters.StepTriesCount; i++)
        {
            var initialQ = GetInitialQ(p);
            if (TryGenerateQByAdding(p, initialQ, out q))
            {
                return true;
            }
        }

        return false;
    }

    private BigInteger GetInitialQ(BigInteger p)
    {
        var pSecondSignificantByte = p.ToByteArray()[^2];

        var qBytes = new byte[_parameters.ByteCount + 1];
        _random.NextBytes(qBytes);
        PrepareBytes(qBytes);
        qBytes[^2] ^= (byte)(~(pSecondSignificantByte ^ qBytes[^2]) & 0b01000000);

        return new BigInteger(qBytes);
    }

    private bool TryGenerateQByAdding(BigInteger p, BigInteger initialQ, out BigInteger q)
    {
        q = initialQ;

        for (var i = 0; i < _parameters.StepTriesCount; i++)
        {
            if (p != q
                && !HasWienerAttackVulnerability(p, q)
                && HasEnoughDifference(p, q)
                && _primalityTester.TestIsPrime(q, _parameters.PrimalityProbability))
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

    private static bool HasWienerAttackVulnerability(BigInteger a, BigInteger b)
    {
        if (a > b)
        {
            (a, b) = (b, a);
        }

        return b <= 2 * a;
    }

    private bool HasEnoughDifference(BigInteger a, BigInteger b)
    {
        if (a > b)
        {
            (a, b) = (b, a);
        }

        var diff = b - a;
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