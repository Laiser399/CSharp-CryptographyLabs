using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Exceptions;

namespace Module.RSA.Services.Abstract;

public interface IRSAAttackService
{
    /// <summary>
    /// Пытается разложить модуль на два множителя
    /// </summary>
    /// <exception cref="ArgumentException">Модуль меньше или равен 1.</exception>
    /// <exception cref="FactorizationException">Модуль является простым числом. Или, возможно, произошла неизвестная ошибка)</exception>
    /// <exception cref="OperationCanceledException">Кого-то отменили.</exception>
    [Obsolete]
    Task<FactorizationResult> FactorizeModulusAsync(BigInteger modulus, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Совершает атаку на RSA
    /// </summary>
    /// <param name="publicExponent">Публичная экспонента</param>
    /// <param name="modulus">Модуль</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Возвращает дешифрующую экпоненту</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="CryptographyAttackException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<BigInteger> AttackAsync(
        BigInteger publicExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken = null);
}