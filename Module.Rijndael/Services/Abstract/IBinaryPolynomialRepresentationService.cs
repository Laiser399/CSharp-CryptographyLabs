namespace Module.Rijndael.Services.Abstract;

public interface IBinaryPolynomialRepresentationService
{
    string ToStringAsPolynomial(uint value);

    bool TryParseAsPolynomial(string polynomial, out uint value);
}