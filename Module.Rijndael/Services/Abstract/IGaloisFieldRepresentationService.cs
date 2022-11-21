namespace Module.Rijndael.Services.Abstract;

public interface IGaloisFieldRepresentationService
{
    string ToStringAsPolynomial(byte value);

    bool TryParseAsPolynomial(string polynomial, out byte value);
}