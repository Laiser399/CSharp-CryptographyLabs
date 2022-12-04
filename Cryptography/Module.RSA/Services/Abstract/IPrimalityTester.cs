using System.Numerics;

namespace Module.RSA.Services.Abstract;

public interface IPrimalityTester
{
    /// <summary>
    /// Производит вероятностную проверку на то, является ли число простым.
    /// </summary>
    /// <returns>
    /// true: Число простое с заданной вероятностью
    /// <br/>
    /// false: Число (точно) составное
    /// </returns>
    bool TestIsPrime(BigInteger value);
}