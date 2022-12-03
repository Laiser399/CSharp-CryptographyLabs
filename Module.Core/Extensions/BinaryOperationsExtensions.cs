namespace Module.Core.Extensions;

public static class BinaryOperationsExtensions
{
    /// <summary>
    /// Вычисляет битовую длину числа.
    /// Например для числа 0b00010101 результат будет равен 5.
    /// </summary>
    public static int GetBitLength(this byte value)
    {
        return GetBitLength((ulong)value);
    }

    /// <inheritdoc cref="GetBitLength(byte)"/>
    public static int GetBitLength(this ushort value)
    {
        return GetBitLength((ulong)value);
    }

    public static int GetBitLength(this uint value)
    {
        return GetBitLength((ulong)value);
    }

    /// <inheritdoc cref="GetBitLength(byte)"/>
    public static int GetBitLength(this ulong value)
    {
        var result = 0;
        while (value > 0)
        {
            result++;
            value >>= 1;
        }

        return result;
    }
}