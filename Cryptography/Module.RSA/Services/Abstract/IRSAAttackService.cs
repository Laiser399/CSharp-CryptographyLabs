using System.Numerics;
using Module.RSA.Exceptions;

namespace Module.RSA.Services.Abstract;

public interface IRSAAttackService
{
    /// <summary>
    /// Совершает атаку на RSA
    /// </summary>
    /// <param name="publicExponent">Публичная экспонента</param>
    /// <param name="modulus">Модуль</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Возвращает дешифрующую экпоненту</returns>
    /// <exception cref="ArgumentException">Экспонента или модуль меньше 2</exception>
    /// <exception cref="CryptographyAttackException">Невозможно провести атаку</exception>
    /// <exception cref="OperationCanceledException">Кого-то отменили</exception>
    Task<BigInteger> AttackAsync(
        BigInteger publicExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken = null);
}