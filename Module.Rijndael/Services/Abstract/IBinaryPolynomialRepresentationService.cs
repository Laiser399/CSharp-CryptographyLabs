namespace Module.Rijndael.Services.Abstract;

public interface IBinaryPolynomialRepresentationService
{
    /// <summary>
    /// Приводит бинарное представление полинома к виду строки.
    /// Пример: 0b11001 -> "x^4 + x^3 + 1"
    /// </summary>
    string ToString(byte value);

    /// <inheritdoc cref="ToString(byte)"/>
    string ToString(ulong value);

    /// <summary>
    /// Парсит полином в бинарное представление.
    /// Пример: "x^4 + x^3 + 1" -> 0b11001 
    /// </summary>
    bool TryParse(string polynomial, out byte value);

    ///<inheritdoc cref="TryParse(string,out byte)"/>
    bool TryParse(string polynomial, out uint value);
}