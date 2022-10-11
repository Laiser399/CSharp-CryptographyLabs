using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Exceptions;
using Module.RSA.Extensions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSAFactorizationAttackService : IRSAAttackService
{
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;

    public RSAFactorizationAttackService(IBigIntegerCalculationService bigIntegerCalculationService)
    {
        _bigIntegerCalculationService = bigIntegerCalculationService;
    }

    public async Task<BigInteger> AttackAsync(
        BigInteger publicExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken = null)
    {
        ValidateModulus(modulus);

        if ((modulus & 1) == 0)
        {
            return GetPrivateExponent(publicExponent, 2, modulus >> 1);
        }

        var modulusSquareRoot = _bigIntegerCalculationService.SquareRoot(modulus);

        if (modulusSquareRoot * modulusSquareRoot == modulus)
        {
            return GetPrivateExponent(publicExponent, modulusSquareRoot, modulusSquareRoot);
        }

        if ((modulusSquareRoot & 1) == 0)
        {
            modulusSquareRoot--;
        }

        var privateExponent = await Task.Run(() =>
        {
            for (var i = modulusSquareRoot; i > 2; i -= 2)
            {
                cancellationToken?.ThrowIfCancellationRequested();

                if (modulus % i == 0)
                {
                    return GetPrivateExponent(publicExponent, i, modulus / i);
                }
            }

            return (BigInteger?)null;
        });

        return privateExponent ?? throw new CryptographyAttackException("Factors not found. Maybe modulus is prime.");
    }

    private static void ValidateModulus(BigInteger modulus)
    {
        if (modulus < 2)
        {
            throw new ArgumentException($"Invalid modulus value: {modulus}.");
        }

        if (modulus == 2)
        {
            throw new CryptographyAttackException("Modulus equal 2. 2 is prime.");
        }
    }

    private BigInteger GetPrivateExponent(BigInteger publicExponent, BigInteger p, BigInteger q)
    {
        var phiModulus = (p - 1) * (q - 1);

        var gcd = _bigIntegerCalculationService.GreatestCommonDivisor(
            publicExponent,
            phiModulus,
            out var privateExponent,
            out _
        );
        if (gcd != 1)
        {
            throw new CryptographyAttackException(
                "Something went wrong. " +
                $"Public exponent and Euler function of modulus has greatest common divisor equal {gcd}."
            );
        }

        return privateExponent.NormalizedMod(phiModulus);
    }

    [Obsolete]
    public async Task<FactorizationResult> FactorizeModulusAsync(
        BigInteger modulus,
        CancellationToken? cancellationToken)
    {
        throw new NotSupportedException();
    }
}