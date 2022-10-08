using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSAAttackService : IRSAAttackService
{
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;

    public RSAAttackService(IBigIntegerCalculationService bigIntegerCalculationService)
    {
        _bigIntegerCalculationService = bigIntegerCalculationService;
    }

    public Task<FactorizationResult> FactorizeModulusAsync(
        BigInteger modulus,
        CancellationToken? cancellationToken)
    {
        ValidateModulus(modulus);

        if ((modulus & 1) == 0)
        {
            return Task.FromResult(new FactorizationResult(2, modulus >> 1));
        }

        var modulusSquareRoot = _bigIntegerCalculationService.SquareRoot(modulus);

        if (modulusSquareRoot * modulusSquareRoot == modulus)
        {
            return Task.FromResult(new FactorizationResult(modulusSquareRoot, modulusSquareRoot));
        }

        if ((modulusSquareRoot & 1) == 0)
        {
            modulusSquareRoot--;
        }

        return Task.Run(() =>
        {
            for (var i = modulusSquareRoot; i > 2; i -= 2)
            {
                cancellationToken?.ThrowIfCancellationRequested();

                if (modulus % i == 0)
                {
                    return new FactorizationResult(i, modulus / i);
                }
            }

            throw new FactorizationException("Factors not found. Maybe modulus is prime.");
        });
    }

    private static void ValidateModulus(BigInteger modulus)
    {
        if (modulus < 2)
        {
            throw new ArgumentException($"Invalid modulus value: {modulus}.");
        }

        if (modulus == 2)
        {
            throw new FactorizationException("Modulus equal 2. 2 is prime.");
        }
    }
}