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
    Task<FactorizationResult> FactorizeModulusAsync(BigInteger modulus, CancellationToken? cancellationToken = null);
}