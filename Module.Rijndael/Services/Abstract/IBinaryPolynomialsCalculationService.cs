namespace Module.Rijndael.Services.Abstract;

public interface IBinaryPolynomialsCalculationService
{
    ulong Multiply(uint a, uint b);

    uint Divide(uint a, uint b, out uint residual);

    /// <summary>
    /// Вычисляет и возвращает НОД, а за одно и коэффициенты такие, что НОД = x * a + y * b.
    /// </summary>
    uint GreatestCommonDivisor(uint a, uint b, out uint x, out uint y);
}