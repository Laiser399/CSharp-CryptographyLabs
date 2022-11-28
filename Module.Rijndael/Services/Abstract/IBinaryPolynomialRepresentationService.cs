namespace Module.Rijndael.Services.Abstract;

public interface IBinaryPolynomialRepresentationService
{
    string ToStringAsPolynomial(ulong value);

    bool TryParseAsPolynomial(string polynomial, out uint value);
}