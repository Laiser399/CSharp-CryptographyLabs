using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IContinuedFractionService
{
    /// <summary>
    /// Вычисляет непрерывную дробь для числа, заданного в виде дроби numerator / denominator.
    /// </summary>
    /// <exception cref="ArgumentException">Числитель меньше нуля или знаменатель меньше или равен нулю.</exception>
    IEnumerable<BigInteger> EnumerateContinuedFraction(BigInteger numerator, BigInteger denominator);
}