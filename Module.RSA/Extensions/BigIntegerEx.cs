using System.Numerics;

namespace Module.RSA.Extensions;

public static class BigIntegerEx
{
    /// <summary>
    /// Возвращает кол-во бит, исключая старшие нулевые биты и бит знака
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetBitCount(this BigInteger value)
    {
        var bytes = value.ToByteArray();

        var bitCount = 0;
        var mostSignificantByte = bytes[^1] & 0b01111111;
        while (mostSignificantByte > 0)
        {
            mostSignificantByte >>= 1;
            bitCount++;
        }

        bitCount += (bytes.Length - 1) * 8;

        return bitCount;
    }

    /// <summary>
    /// Вычисляет остаток от деления.
    /// Приводит результат в диапазон [0; divider - 1].
    /// </summary>
    public static BigInteger NormalizedMod(this BigInteger value, BigInteger divider)
    {
        var result = value % divider;
        if (result < 0)
        {
            result += divider;
        }

        return result;
    }
}