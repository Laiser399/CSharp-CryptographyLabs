using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class WienerAttackService : IRSAAttackService
{
    private readonly IRandomProvider _randomProvider;
    private readonly IBigIntegerCalculationService _bigIntegerCalculationService;
    private readonly IContinuedFractionService _continuedFractionService;
    private readonly IConvergingFractionsService _convergingFractionsService;
    private readonly IWienerAttackStatisticsCollector? _wienerAttackStatisticsCollector;

    public WienerAttackService(
        IRandomProvider randomProvider,
        IBigIntegerCalculationService bigIntegerCalculationService,
        IContinuedFractionService continuedFractionService,
        IConvergingFractionsService convergingFractionsService,
        IWienerAttackStatisticsCollector? wienerAttackStatisticsCollector = null)
    {
        _randomProvider = randomProvider;
        _bigIntegerCalculationService = bigIntegerCalculationService;
        _continuedFractionService = continuedFractionService;
        _convergingFractionsService = convergingFractionsService;
        _wienerAttackStatisticsCollector = wienerAttackStatisticsCollector;
    }

    public async Task<BigInteger> AttackAsync(
        BigInteger publicExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken)
    {
        ValidateArguments(publicExponent, modulus);

        var message = GenerateTestMessage(modulus);
        var encrypted = _bigIntegerCalculationService.BinPowMod(message, publicExponent, modulus);

        var potentialPrivateExponents = EnumeratePotentialPrivateExponents(publicExponent, modulus);

        foreach (var potentialPrivateExponent in potentialPrivateExponents)
        {
            cancellationToken?.ThrowIfCancellationRequested();

            _wienerAttackStatisticsCollector?.IncreaseExponentsCheckedCount();

            if (await IsHitAsync(message, encrypted, potentialPrivateExponent, modulus, cancellationToken))
            {
                return potentialPrivateExponent;
            }
        }

        throw new CryptographyAttackException("Private exponent not found.");
    }

    private static void ValidateArguments(BigInteger publicExponent, BigInteger modulus)
    {
        if (publicExponent <= 0)
        {
            throw new ArgumentException("Public exponent lower or equal 0.", nameof(publicExponent));
        }

        if (modulus <= 1)
        {
            throw new ArgumentException("Modulus lower or equal 1.", nameof(modulus));
        }
    }

    private BigInteger GenerateTestMessage(BigInteger modulus)
    {
        var byteCount = modulus.GetByteCount(true) - 1;
        var bytes = new byte[byteCount];
        _randomProvider.Random.NextBytes(bytes);
        return new BigInteger(bytes, true);
    }

    private IEnumerable<BigInteger> EnumeratePotentialPrivateExponents(BigInteger publicExponent, BigInteger modulus)
    {
        var continuedFraction = _continuedFractionService.EnumerateContinuedFraction(publicExponent, modulus);
        var convergingFractions = _convergingFractionsService.EnumerateConvergingFractions(continuedFraction);
        return convergingFractions.Select(x => x.Q);
    }

    private Task<bool> IsHitAsync(
        BigInteger message,
        BigInteger encryptedMessage,
        BigInteger privateExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken)
    {
        return Task.Run(
            () =>
            {
                var decrypted = _bigIntegerCalculationService.BinPowMod(encryptedMessage, privateExponent, modulus);

                return message == decrypted;
            },
            cancellationToken ?? CancellationToken.None
        );
    }
}