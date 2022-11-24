using System.Numerics;
using Module.RSA.Entities.Abstract;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Extensions;

public static class RSAAttackServiceExtensions
{
    /// <summary>
    /// Совершает атаку на RSA
    /// </summary>
    /// <returns>Возвращает дешифрующую экпоненту</returns>
    /// <exception cref="ArgumentException">Экспонента или модуль меньше 2</exception>
    /// <exception cref="CryptographyAttackException">Невозможно провести атаку</exception>
    /// <exception cref="OperationCanceledException">Кого-то отменили</exception>
    public static Task<BigInteger> AttackAsync(
        this IRSAAttackService rsaAttackService,
        IRSAKey publicKey,
        CancellationToken? cancellationToken = null)
    {
        return rsaAttackService.AttackAsync(publicKey.Exponent, publicKey.Modulus, cancellationToken);
    }
}